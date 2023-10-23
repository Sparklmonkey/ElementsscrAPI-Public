using ElementscrAPI.Entities;

namespace ElementscrAPI.Services;

public interface IPvpRoomService
{
    Task<string> CreatePvpRoom(ConnectedUser host, bool isPvpOne, bool isOpenRoom);
    Task<PvpRoom> JoinRandomRoom(ConnectedUser client, bool isPvpOne);
    Task OpenPvpRoom(string roomId, bool isPvpOne);
    Task ClosePvpRoom(string roomId, bool isPvpOne);
    Task<PvpRoom> JoinRoom(ConnectedUser client, string hostUsername, bool isPvpOne);
    Task LeaveRoom(string playerUsername, bool isPvpOne);
}