using ElementscrAPI.Entities;
using ElementscrAPI.Helpers;

namespace ElementscrAPI.Services;

public class PvpRoomService : IPvpRoomService
{
    private readonly Dictionary<string, PvpRoom> _pvpRoomOne
        = new Dictionary<string, PvpRoom>();

    private readonly Dictionary<string, PvpRoom> _pvpRoomTwo
        = new Dictionary<string, PvpRoom>();

    public async Task<string> CreatePvpRoom(ConnectedUser host, bool isPvpOne, bool isOpenRoom)
    {
        var guid = Guid.NewGuid().ToString();
        var newRoom = new PvpRoom()
        {
            FirstConnectedPlayer = host,
            SecondConnectedPlayer = null,
            IsOpenRoom = isOpenRoom
        };
        if (isPvpOne)
        {
            _pvpRoomOne.Add(guid, newRoom);
        }
        else
        {
            _pvpRoomTwo.Add(guid, newRoom);
        }

        return guid;
    }

    public async Task OpenPvpRoom(string roomId, bool isPvpOne)
    {
        if (isPvpOne)
        {
            _pvpRoomOne[roomId].IsOpenRoom = true;
        }
        else
        {
            _pvpRoomTwo[roomId].IsOpenRoom = true;
        }
    }

    public async Task ClosePvpRoom(string roomId, bool isPvpOne)
    {
        if (isPvpOne)
        {
            _pvpRoomOne[roomId].IsOpenRoom = false;
        }
        else
        {
            _pvpRoomTwo[roomId].IsOpenRoom = false;
        }
    }

    public async Task<PvpRoom> JoinRandomRoom(ConnectedUser client, bool isPvpOne)
    {
        if (isPvpOne)
        {
            var room = _pvpRoomOne.Where(x => x.Value.SecondConnectedPlayer is not null && x.Value.IsOpenRoom).FirstOrDefault();
            room.Value.SecondConnectedPlayer = client;
            _pvpRoomOne[room.Key] = room.Value;
            return room.Value;
        }
        else
        {
            var room = _pvpRoomTwo.Where(x => x.Value.SecondConnectedPlayer is not null && x.Value.IsOpenRoom).FirstOrDefault();
            room.Value.SecondConnectedPlayer = client;
            _pvpRoomTwo[room.Key] = room.Value;
            return room.Value;
        }
    }


    public async Task<PvpRoom> JoinRoom(ConnectedUser client, string hostUsername, bool isPvpOne)
    {
        if (isPvpOne)
        {
            var room = _pvpRoomOne.Where(x => x.Value.FirstConnectedPlayer.Username == hostUsername).FirstOrDefault();
            room.Value.SecondConnectedPlayer = client;
            _pvpRoomOne[room.Key] = room.Value;
            return room.Value;
        }
        else
        {
            var room = _pvpRoomTwo.Where(x => x.Value.FirstConnectedPlayer.Username == hostUsername).FirstOrDefault();
            room.Value.SecondConnectedPlayer = client;
            _pvpRoomTwo[room.Key] = room.Value;
            return room.Value;
        }
    }

    public async Task LeaveRoom(string playerUsername, bool isPvpOne)
    {
        if (isPvpOne)
        {
            var isHost = _pvpRoomOne.IsHostPlayer(playerUsername);
            if (isHost)
            {
                var room = _pvpRoomOne.Where(x => x.Value.FirstConnectedPlayer.Username == playerUsername).FirstOrDefault();
                _pvpRoomOne.Remove(room.Key);
            }
            else
            {
                var room = _pvpRoomOne.Where(x => x.Value.FirstConnectedPlayer.Username == playerUsername).FirstOrDefault();
                room.Value.SecondConnectedPlayer = null;
                _pvpRoomOne[room.Key] = room.Value;
            }
        }
        else
        {var isHost = _pvpRoomTwo.IsHostPlayer(playerUsername);
            if (isHost)
            {
                var room = _pvpRoomTwo.Where(x => x.Value.FirstConnectedPlayer.Username == playerUsername).FirstOrDefault();
                _pvpRoomTwo.Remove(room.Key);
            }
            else
            {
                var room = _pvpRoomTwo.Where(x => x.Value.FirstConnectedPlayer.Username == playerUsername).FirstOrDefault();
                room.Value.SecondConnectedPlayer = null;
                _pvpRoomTwo[room.Key] = room.Value;
            }
        }
    }
}
