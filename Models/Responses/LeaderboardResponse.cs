using ElementscrAPI.Entities;

namespace ElementscrAPI.Models.Responses;

public class LeaderboardResponse
{
    public List<LeaderboardObject> leaderboardList { get; set; }

    public LeaderboardResponse()
    {
        leaderboardList = new();
    }
}
public class LeaderboardObject
{
    public string Username { get; set; }
    public int PlayerScore { get; set; }
    public int CardCount { get; set; }
    public int OverallWins { get; set; }
    public int OverallLoses { get; set; }
    public int Electrum { get; set; }
    public GameStats GameStats { get; set; }

    public LeaderboardObject()
    {
        Username = "";
        PlayerScore = 0;
        CardCount = 0;
        OverallWins = 0;
        OverallLoses = 0;
        Electrum = 0;
        GameStats = new();
    }
}