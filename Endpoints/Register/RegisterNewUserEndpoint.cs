using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Filters;
using ElementscrAPI.Helpers;
using ElementscrAPI.Models;
using ElementscrAPI.Models.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.Register;

[HttpPost("api/register/new"), AllowAnonymous]
public class RegisterNewUserEndpoint : Endpoint<RegisterRequest, LoginResponse>
{
    private readonly PlayerDataContext _context;
    private readonly IJwtAuth _jwtAuth;

    public RegisterNewUserEndpoint(PlayerDataContext context, IJwtAuth jwtAuth)
    {
        _context = context;
        _jwtAuth = jwtAuth;
    }

    public override async Task HandleAsync(RegisterRequest request, CancellationToken ct)
    {
        if (!request.Username.UsernameCheck())
        {
            await SendOkAsync(new LoginResponse()
            {
                ErrorMessage = ErrorCases.UsernameInvalid
            }, ct);
        }
        
        if (!request.Password.PasswordCheck())
        {
            await SendOkAsync(new LoginResponse()
            {
                ErrorMessage = ErrorCases.PasswordInvalid
            }, ct);
        }

        var existingUser = await _context.PlayerData.Where(x => x.UserData.Username == request.Username).FirstOrDefaultAsync();
        if (existingUser is not null)
        {
            await SendOkAsync(new LoginResponse()
            {
                ErrorMessage = ErrorCases.UserNameInUse
            }, ct);
        }

        var salt = ExtendMethods.GenerateRndSalt();
        
        var userData = new Entities.UserData()
        {
            EmailAddress = request.Email,
            Username = request.Username,
            Salt = salt,
            Password = request.Password.EncryptPassword(salt),
            LastLogin = DateTime.UtcNow
        };

        var saveData = new SavedData();
        var gameStats = new GameStats();
        var redeemCodes = new List<string>();
        var accountId = Guid.NewGuid().ToString();
        var accessToken = _jwtAuth.AccessTokenCreation(userData.Username, accountId);
        var token = _jwtAuth.Authentication(userData.Username, accountId, "no");

        var playerData = new PlayerData()
        {
            UserData = userData,
            SavedData = saveData,
            GameStats = gameStats,
            RedeemCodes = redeemCodes,
            AccessToken = accessToken,
            AccountId = accountId
        };
        var list = await _context.PlayerData.ToListAsync();
        playerData.Id = list.Count + 1;
        _context.PlayerData.Add(playerData);
        await _context.SaveChangesAsync();
        await SendOkAsync(new LoginResponse()
        {
            AccessToken = accessToken,
            EmailAddress = request.Email,
            ErrorMessage = ErrorCases.AllGood,
            SavedData = saveData,
            Token = token
        }, ct );
    }
}