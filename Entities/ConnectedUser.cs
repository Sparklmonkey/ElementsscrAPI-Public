namespace ElementscrAPI.Entities;

public class ConnectedUser
{
    public string AccountId { get; set; }
    public string Username { get; set; }
    public string ConnectionId { get; set; }
    public List<string> DeckList { get; set; }
    public int ElementMark { get; set; }
    public int Score { get; set; }
    public int Win { get; set; }
    public int Lose { get; set; }
}
