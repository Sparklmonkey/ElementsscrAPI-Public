using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Models.Requests;
using ElementscrAPI.Models.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.UserData;

[HttpPost("api/user-data/leaderboard"), AllowAnonymous]
public class GetLeaderboardEndpoint : Endpoint<LeaderboardRequest, LeaderboardResponse>
{

    // public GetLeaderboardEndpoint(PlayerDataContext context, IMemoryCache memoryCache)
    // {
    //     _context = context;
    //     _memoryCache = memoryCache;
    // }
    
    public override async Task HandleAsync(LeaderboardRequest req, CancellationToken ct)
    {
        await SendNoContentAsync(ct);
    }

   
}
