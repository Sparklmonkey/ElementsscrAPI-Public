namespace ElementscrAPI.Entities;

public class Stat
{
    public string Name { get; set; }
    public int Wins { get; set; }
    public int Loses { get; set; }

    public Stat()
    {
        Name = "";
        Wins = 0;
        Loses = 0;
    }
    public Stat(string aiName)
    {
        Name = aiName;
        Wins = 0;
        Loses = 0;
    }

    public void AddCount(bool isWin)
    {
        if (isWin)
        {
            Wins++;
        }
        else
        {
            Loses++;
        }
    }
}