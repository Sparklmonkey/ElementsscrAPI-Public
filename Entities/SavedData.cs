namespace ElementscrAPI.Entities;

public class SavedData
{ 
    public int MarkElement { get; set; }
    public List<string> CurrentDeck { get; set; }
    public List<string> InventoryCards { get; set; }
    public int Electrum { get; set; }
    public int GamesWon { get; set; }

    public int GamesLost { get; set; }

    public int PlayerScore { get; set; }

    public int CurrentQuestIndex { get; set; }

    public bool PlayedOracleToday { get; set; }

    public bool HasDefeatedLevel0 { get; set; }

    public bool HasDefeatedLevel1 { get; set; }

    public bool HasDefeatedLevel2 { get; set; }
    public bool RemovedCardFromDeck { get; set; }
    public bool HasBoughtCardBazaar { get; set; }
    public bool HasSoldCardBazaar { get; set; }
    public DateTime OracleLastPlayed { get; set; }
    public List<string> ArenaT50Deck { get; set; }
    public int ArenaT50Mark { get; set; }

    public int ArenaWins { get; set; }

    public int ArenaLosses { get; set; }

    public SavedData()
    {
        MarkElement = 0;
        CurrentDeck = new();
        InventoryCards = new();
        Electrum = 0;
        GamesWon = 0;
        GamesLost = 0;
        PlayerScore = 0;
        CurrentQuestIndex = 0;
        PlayedOracleToday = false;
        OracleLastPlayed = DateTime.Now;
        HasDefeatedLevel0 = false;
        RemovedCardFromDeck = false;
        HasBoughtCardBazaar = false;
        HasSoldCardBazaar = false;
        HasDefeatedLevel1 = false;
        HasDefeatedLevel2 = false;
        ArenaT50Deck = new();
        ArenaT50Mark = 0;
        ArenaWins = 0;
        ArenaLosses = 0;
    }
}