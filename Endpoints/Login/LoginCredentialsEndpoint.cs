using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Filters;
using ElementscrAPI.Helpers;
using ElementscrAPI.Models;
using ElementscrAPI.Models.Requests;
using ElementscrAPI.Repositories;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.Login;

[HttpPost("api/login/credential"), AllowAnonymous]
public class LoginCredentialsEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly PlayerDataContext _context;
    private readonly IJwtAuth _jwtAuth;

    public LoginCredentialsEndpoint(PlayerDataContext context, IJwtAuth jwtAuth)
    {
        _context = context;
        _jwtAuth = jwtAuth;
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var playerData = await _context.PlayerData.Where(x => x.UserData.Username == req.Username).FirstOrDefaultAsync();

        if (playerData is null)
        {
            await SendOkAsync(new LoginResponse()
            {
                ErrorMessage = ErrorCases.UserDoesNotExist
            }, ct);
        }

        var hashPass = req.Password.EncryptPassword(playerData.UserData.Salt);

        if(playerData.UserData.Password != hashPass)
        {
            await SendOkAsync(new LoginResponse()
            {
                ErrorMessage = ErrorCases.IncorrectCredentials
            }, ct);
        }

        var accountId = playerData.AccountId;
        var jwtToken = _jwtAuth.Authentication(playerData.UserData.Username, accountId, "no");
        var accessToken = _jwtAuth.AccessTokenCreation(playerData.UserData.Username, accountId);

        playerData.UserData.LastLogin = DateTime.UtcNow;
        playerData.AccessToken = accessToken;
        await _context.SaveChangesAsync();
        
        await SendOkAsync(new LoginResponse()
        {
            Username = playerData.UserData.Username,
            EmailAddress = playerData.UserData.EmailAddress,
            SavedData = playerData.SavedData,
            ErrorMessage = ErrorCases.AllGood,
            Token = jwtToken,
            AccessToken = playerData.AccessToken
        }, ct);
    }
}