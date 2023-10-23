namespace ElementscrAPI.Entities;

public class PvpRoom
{
        public ConnectedUser FirstConnectedPlayer { get; set; }
        public ConnectedUser? SecondConnectedPlayer { get; set; }
        public bool IsOpenRoom { get; set; }
}