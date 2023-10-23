using ElementscrAPI.Data;
using ElementscrAPI.Filters;
using ElementscrAPI.Models;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.GameStatsEndPoint;


[HttpPut("api/game-stats/update")]
public class UpdateGameStatsEndpoint: Endpoint<GameStatRequest, GameStatResponse>
{
    private readonly IJwtAuth _jwtAuth;
    private readonly PlayerDataContext _context;

    public UpdateGameStatsEndpoint(IJwtAuth jwtAuth, PlayerDataContext context)
    {
        _context = context;
        _jwtAuth = jwtAuth;
    }

    public override async Task HandleAsync(GameStatRequest request, CancellationToken ct)
    {
        var accountId = _jwtAuth.GetAccountId(request.AccessToken.Replace("Bearer ", ""));
        var player = await _context.PlayerData.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();

        if (player is null)
        {
            await SendOkAsync(new GameStatResponse(){WasSuccess = false});
        }

        player.GameStats.UpdateValues(request);
        await _context.SaveChangesAsync();
        await SendOkAsync(new GameStatResponse(){WasSuccess = true});
    }
}