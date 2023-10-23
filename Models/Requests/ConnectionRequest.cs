namespace ElementscrAPI.Models.Requests;

public class ConnectionRequest
{
    public string AccountId { get; set; }
    public bool IsPvpOne { get; set; }
    public bool IsOpenRoom { get; set; }
    public List<string> PvpDeck { get; set; }
    public int Mark { get; set; }
}