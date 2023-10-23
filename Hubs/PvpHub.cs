using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Models.Requests;
using ElementscrAPI.Models.Responses;
using ElementscrAPI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ElementscrAPI.Hubs;

public class PvpHub : Hub
{
    private readonly PlayerDataContext _context;
    private readonly IPvpRoomService _pvpRoomService;

    public PvpHub(PlayerDataContext context, IPvpRoomService pvpRoomService)
    {
        _context = context;
        _pvpRoomService = pvpRoomService;
    }
    
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"Connected on {Context.ConnectionId}" );
        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveConnId", Context.ConnectionId);
    }

    public async Task StartPvpRoom(string message)
    {
        var connectionRequest = JsonConvert.DeserializeObject<ConnectionRequest>(message);
        var playerData = await _context.PlayerData.Where(x => x.AccountId == connectionRequest.AccountId).FirstOrDefaultAsync();

        if (playerData is null)
        {
            var responseFailed = new ConnectionResponse()
            {
                ServerMessage = "There was an issue connecting to the Pvp Service. Please try again later"
            };
            await Clients.Client(Context.ConnectionId).SendAsync("RoomCreated", JsonConvert.SerializeObject(responseFailed));
            return;
        }

        var user = new ConnectedUser()
        {
            Username = playerData.UserData.Username,
            AccountId = connectionRequest.AccountId,
            ConnectionId = Context.ConnectionId,
            DeckList = connectionRequest.PvpDeck,
            ElementMark = connectionRequest.Mark,
            Lose = playerData.SavedData.GamesLost,
            Score = playerData.SavedData.PlayerScore,
            Win = playerData.SavedData.GamesWon
        };

        var roomId = await _pvpRoomService.CreatePvpRoom(user, connectionRequest.IsPvpOne, connectionRequest.IsOpenRoom);

        var response = new ConnectionResponse()
        {
            ConnectionId = roomId,
            ServerMessage =
                "Connection was successful. You can share this code with someone if you want to play against them."
        };
        await Clients.Client(Context.ConnectionId).SendAsync("RoomCreated", JsonConvert.SerializeObject(response));
    }
    
    public async Task ChangePvpRoomStatus(string message)
    {
        
    }

    public async Task JoinRandomPvpRoom()
    {
        
    }

    public async Task JoinPvpRoom()
    {
        
    }

    public async Task SendQuantaSpendAsync(string message)
    {
        var routeOb = JsonConvert.DeserializeObject<dynamic>(message);
        Console.WriteLine($"Message Received on {Context.ConnectionId}" );
    }
}

public class QuantaGeneratedMessage
{
    public List<QuantaMessage> QuantaGenerated { get; set; }
}

public class QuantaMessage
{
    public int Element { get; set; }
    public int Count { get; set; }
}

public class PlayedCardFromHandMessage
{
    public Id CardPlayed { get; set; }
    public List<QuantaMessage> QuantaSpent { get; set; }
    public Id Target { get; set; }
}

public class Id 
{
    public int Owner { get; set; }
    public int Field { get; set; }
    public int Index { get; set; }
}