using ElementscrAPI.Helpers;
using ElementscrAPI.Models;

namespace ElementscrAPI.Entities;

[Serializable]
public class GameStats
{
    public List<Stat> AiLevel0 { get; set; }
    public List<Stat> AiLevel1 { get; set; }
    public List<Stat> AiLevel2 { get; set; }
    public List<Stat> AiLevel3 { get; set; }
    public List<Stat> AiLevel4 { get; set; }
    public List<Stat> AiLevel5 { get; set; }
    public List<Stat> ArenaT50 { get; set; }
    public List<Stat> PvpOne { get; set; }
    public List<Stat> PvpTwo { get; set; }
    public GameStats()
    {
        AiLevel0 = new();
        AiLevel1 = new();
        AiLevel2 = new();
        AiLevel3 = new();
        AiLevel4 = new();
        AiLevel5 = new();
        PvpOne = new() { new("Sparklmonkey") };
        PvpTwo = new() { new("Sparklmonkey") };
        ArenaT50 = new() { new("Sparklmonkey") };
        foreach (var item in StatHelper.ElementStringList)
        {
            AiLevel0.Add( new(item));
            AiLevel1.Add( new(item));
        }
        foreach (var item in StatHelper.Ai2List)
        {
            AiLevel2.Add( new(item));
            AiLevel3.Add( new(item));
        }
        foreach (var item in StatHelper.ElderPrefix)
        {
            foreach (var item2 in StatHelper.ElderSuffix)
            {
                AiLevel4.Add( new($"{item}{item2}"));
            }
        }
        foreach (var item in StatHelper.FalseGodNameList)
        {
            AiLevel5.Add(new(item));
        }
    }

    public void UpdateValues(GameStatRequest stats)
    {
        switch (stats.AiLevel)
        {
            case 0:
                AiLevel0.Find(x => x.Name == stats.AiName)?.AddCount(stats.IsWin);
                break;
            case 1:
                AiLevel1.Find(x => x.Name == stats.AiName)?.AddCount(stats.IsWin);
                break;
            case 2:
                AiLevel2.Find(x => x.Name == stats.AiName)?.AddCount(stats.IsWin);
                break;
            case 3:
                AiLevel3.Find(x => x.Name == stats.AiName)?.AddCount(stats.IsWin);
                break;
            case 4:
                AiLevel4.Find(x => x.Name == stats.AiName)?.AddCount(stats.IsWin);
                break;
            case 5:
                AiLevel5.Find(x => x.Name == stats.AiName)?.AddCount(stats.IsWin);
                break;
            case 6:
                if (ArenaT50.Find(x => x.Name == stats.AiName) == null)
                {
                    ArenaT50.Add(new Stat(stats.AiName));
                }
                ArenaT50.Find(x => x.Name == stats.AiName)?.AddCount(stats.IsWin);
                break;
            case 7:
                if (PvpOne.Find(x => x.Name == stats.AiName) == null)
                {
                    PvpOne.Add(new Stat(stats.AiName));
                }
                PvpOne.Find(x => x.Name == stats.AiName)?.AddCount(stats.IsWin);
                break;
            case 8:
                if (PvpTwo.Find(x => x.Name == stats.AiName) == null)
                {
                    PvpTwo.Add(new Stat(stats.AiName));
                }
                PvpTwo.Find(x => x.Name == stats.AiName)?.AddCount(stats.IsWin);
                break;
        }
    }
}