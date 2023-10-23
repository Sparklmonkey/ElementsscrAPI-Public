using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Filters;
using ElementscrAPI.Models;
using ElementscrAPI.Models.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.Login;

[HttpPost("api/login/token"), AllowAnonymous]
public class LoginAccessTokenEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly PlayerDataContext _context;
    private readonly IJwtAuth _jwtAuth;

    public LoginAccessTokenEndpoint(PlayerDataContext context, IJwtAuth jwtAuth)
    {
        _context = context;
        _jwtAuth = jwtAuth;
    }


    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        if (req.AccessToken == "NotValid")
        {
            await SendOkAsync(new LoginResponse()
            {
                ErrorMessage = ErrorCases.UserDoesNotExist
            }, ct);
        }

        var playerData = await _context.PlayerData.Where(x => x.AccessToken == req.AccessToken)
            .FirstOrDefaultAsync();
        if (playerData is null)
        {
            await SendOkAsync(new LoginResponse()
            {
                ErrorMessage = ErrorCases.UserDoesNotExist
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
            EmailAddress = playerData.UserData.EmailAddress,
            SavedData = playerData.SavedData,
            ErrorMessage = ErrorCases.AllGood,
            Token = jwtToken,
            AccessToken = accessToken
        }, ct);
    }
}