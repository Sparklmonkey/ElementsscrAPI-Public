namespace ElementscrAPI.Entities;

public class PlayerData
{
    public int Id { get; set; }
    public UserData UserData { get; set; }
    public SavedData SavedData { get; set; }
    public GameStats GameStats { get; set; }
    public string AccountId { get; set; }
    public List<string> RedeemCodes { get; set; }
    public string AccessToken { get; set; }
}