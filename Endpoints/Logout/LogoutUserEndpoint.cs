using ElementscrAPI.Data;
using ElementscrAPI.Filters;
using ElementscrAPI.Models;
using ElementscrAPI.Repositories;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.Logout;

[HttpGet("api/logout")]
public class LogoutUserEndpoint : Endpoint<LogoutRequest, bool>
{
    private readonly PlayerDataContext _context;
    private readonly IJwtAuth _jwtAuth;

    public LogoutUserEndpoint(PlayerDataContext context, IJwtAuth jwtAuth)
    {
        _context = context;
        _jwtAuth = jwtAuth;
    }

    public override async Task HandleAsync(LogoutRequest request, CancellationToken ct)
    {
        var accountId = _jwtAuth.GetAccountId(request.AccessToken.Replace("Bearer ", ""));
        var playerData = await _context.PlayerData.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
        if (playerData is null)
        {
            await SendOkAsync(false, ct);
        }
        playerData.AccessToken = "NotValid";
        await _context.SaveChangesAsync();
        await SendOkAsync(true, ct);
    }
}