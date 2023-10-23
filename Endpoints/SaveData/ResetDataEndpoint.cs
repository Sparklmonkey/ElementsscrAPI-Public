using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Filters;
using ElementscrAPI.Models.Requests;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.SaveData;

[HttpPut("api/save-data/reset")]
public class ResetDataEndpoint: Endpoint<SaveDataRequest, SaveDataRequest>
{
    private readonly IJwtAuth _jwtAuth;
    private readonly PlayerDataContext _context;

    public ResetDataEndpoint(IJwtAuth jwtAuth, PlayerDataContext context)
    {
        _context = context;
        _jwtAuth = jwtAuth;
    }

    public override async Task HandleAsync(SaveDataRequest request, CancellationToken ct)
    {
        var accountId = _jwtAuth.GetAccountId(request.AccessToken.Replace("Bearer ", ""));
        var player = await _context.PlayerData.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
        if (player is null)
        {
            await SendOkAsync(new(), ct);
        }

        player.SavedData = new SavedData();
        player.GameStats = new GameStats();

        await _context.SaveChangesAsync();
        await SendOkAsync(new() { SavedData = player.SavedData}, ct);
    }
}