using System.Net;
using Azure.Core;
using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Filters;
using ElementscrAPI.Models;
using ElementscrAPI.Models.Requests;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.SaveData;


[HttpPut("api/save-data/update")]
public class UpdateSaveDataEndpoint: Endpoint<SaveDataRequest, GameStatResponse>
{
    private readonly IJwtAuth _jwtAuth;
    private readonly PlayerDataContext _context;

    public UpdateSaveDataEndpoint(IJwtAuth jwtAuth, PlayerDataContext context)
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
            await SendOkAsync(new(){WasSuccess = false}, ct);
        }
        
        player.SavedData = request.SavedData;
        await _context.SaveChangesAsync();
        await SendOkAsync(new(){WasSuccess = true}, ct);
    }
}