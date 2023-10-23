namespace ElementscrAPI.Entities;

public class ArenaResponse
{
        public List<string> ArenaT50Deck { get; set; }
        public int ArenaT50Mark { get; set; }
        public string Username { get; set; }
        public int PlayerScore { get; set; }
        public int ArenaWins { get; set; }
        public int ArenaLoses { get; set; }
        public int ArenaRank { get; set; }
    
}