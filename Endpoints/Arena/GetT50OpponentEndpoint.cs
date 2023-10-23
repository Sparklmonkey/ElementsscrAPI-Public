using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Filters;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.Arena;

[HttpGet("api/arena/t50opponent")]
public class GetT50OpponentEndpoint: EndpointWithoutRequest<ArenaResponse>
{
    private readonly PlayerDataContext _context;
    private readonly Random _rnd;

    public GetT50OpponentEndpoint(PlayerDataContext context)
    {
        _context = context;
        _rnd = new();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var opponentList = await _context.PlayerData.OrderByDescending(p => p.SavedData.PlayerScore).ToListAsync();
        var index = _rnd.Next(0, opponentList.Count > 50 ? 50 : opponentList.Count);
        var enemy = opponentList[index];

        await SendOkAsync(new()
        {
            Username = enemy.UserData.Username,
            ArenaT50Mark = enemy.SavedData.ArenaT50Mark,
            ArenaT50Deck = enemy.SavedData.ArenaT50Deck.ToList(),
            PlayerScore = enemy.SavedData.PlayerScore,
            ArenaLoses = enemy.SavedData.ArenaLosses,
            ArenaRank = index,
            ArenaWins = enemy.SavedData.ArenaWins
        }, ct);
    }
}