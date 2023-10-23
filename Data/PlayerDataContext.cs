using System.ComponentModel.DataAnnotations.Schema;
using ElementscrAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Data;

public class PlayerDataContext : DbContext
{
    private readonly IConfiguration _config;

    public PlayerDataContext(DbContextOptions<PlayerDataContext> options, IConfiguration config) : base(options)
    {
        _config = config;
    }

    public DbSet<UserDataOld> UserData { get; set; }
    public DbSet<SavedDataOld> SavedData { get; set; }
    public DbSet<AdminUser> AdminUser { get; set; }
    public DbSet<PlayerData> PlayerData { get; set; }
    public DbSet<RedeemCodes> RedeemCodes { get; set; }
    public DbSet<AppInfo> AppInfo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RedeemCodes>()
            .OwnsOne(redeemCode => redeemCode.CodeDetails, builer => { builer.ToJson(); });

        modelBuilder.Entity<PlayerData>().OwnsOne(
            playerData => playerData.UserData,
            builder => { builder.ToJson(); }
        ).OwnsOne(
            playerData => playerData.SavedData,
            builder =>
            {
                builder.ToJson();
                builder.Property(p => p.CurrentDeck)
                    .HasConversion(
                        v => string.Join(" ", v),
                        v => new List<string>(v.Split(" ", StringSplitOptions.RemoveEmptyEntries)));

                builder.Property(p => p.InventoryCards)
                    .HasConversion(
                        v => string.Join(" ", v),
                        v => new List<string>(v.Split(" ", StringSplitOptions.RemoveEmptyEntries)));
                builder.Property(p => p.ArenaT50Deck)
                    .HasConversion(
                        v => string.Join(" ", v),
                        v => new List<string>(v.Split(" ", StringSplitOptions.RemoveEmptyEntries)));
            }
        ).OwnsOne(playerData => playerData.GameStats, builder =>
        {
            builder.ToJson();
            builder.OwnsMany(gameStat => gameStat.AiLevel0);
            builder.OwnsMany(gameStat => gameStat.AiLevel1);
            builder.OwnsMany(gameStat => gameStat.AiLevel2);
            builder.OwnsMany(gameStat => gameStat.AiLevel3);
            builder.OwnsMany(gameStat => gameStat.AiLevel4);
            builder.OwnsMany(gameStat => gameStat.AiLevel5);
            builder.OwnsMany(gameStat => gameStat.PvpOne);
            builder.OwnsMany(gameStat => gameStat.PvpTwo);
            builder.OwnsMany(gameStat => gameStat.ArenaT50);
        });
        modelBuilder.Entity<PlayerData>()
            .Property(e => e.RedeemCodes)
            .HasConversion(
                v => string.Join(" ", v),
                v => new List<string>(v.Split(" ", StringSplitOptions.RemoveEmptyEntries)));
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
         optionsBuilder.UseSqlServer(_config.GetValue<string>("ConnectionStrings:userDataConnection"));
    }
}

public class UserDataOld
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string UserPassKey { get; set; }
    public string Salt { get; set; }
    public string EmailAddress { get; set; }
    public string SavedDataId { get; set; }
}

public class SavedDataOld
{
    public int Id { get; set; }
    public string DeckCards { get; set; }
    public byte MarkElement { get; set; }
    [NotMapped]
    public int MarkElementInt
    {
        get => Convert.ToInt32(MarkElement);
        set => MarkElement = (byte)value;
    }
    public string InventoryCards { get; set; }
    public Int64 Electrum { get; set; }
    public Int64 GamesWon { get; set; }
    public Int64 GamesLost { get; set; }
    public Int64 PlayerScore { get; set; }
    public byte CurrentQuestIndex { get; set; }
    [NotMapped]
    public int CurrentQuestIndexInt
    {
        get => Convert.ToInt32(CurrentQuestIndex);
        set => CurrentQuestIndex = (byte)value;
    }
    public byte? PlayedOracleToday { get; set; }
    public byte? HasDefeatedLevel0 { get; set; }
    public byte? HasDefeatedLevel1 { get; set; }
    public byte? HasDefeatedLevel2 { get; set; }
    public byte? RemovedCardFromDeck { get; set; }
    public byte? HasBoughtCardBazaar { get; set; }
    public byte? HasSoldCardBazaar { get; set; }
    
    [NotMapped]
    public bool PlayedOracleTodayBool
    {
        get => PlayedOracleToday > 0;
        set => PlayedOracleToday = (byte)(value ? 1 : 0);
    }
    [NotMapped]
    public bool HasDefeatedLevel0Bool
    {
        get => HasDefeatedLevel0 > 0;
        set => HasDefeatedLevel0 = (byte)(value ? 1 : 0);
    }
    [NotMapped]
    public bool HasDefeatedLevel1Bool
    {
        get => HasDefeatedLevel1 > 0;
        set => HasDefeatedLevel1 = (byte)(value ? 1 : 0);
    }
    [NotMapped]
    public bool HasDefeatedLevel2Bool
    {
        get => HasDefeatedLevel2 > 0;
        set => HasDefeatedLevel2 = (byte)(value ? 1 : 0);
    }
    [NotMapped]
    public bool RemovedCardFromDeckBool
    {
        get => RemovedCardFromDeck > 0;
        set => RemovedCardFromDeck = (byte)(value ? 1 : 0);
    }
    [NotMapped]
    public bool HasBoughtCardBazaarBool
    {
        get => HasBoughtCardBazaar > 0;
        set => HasBoughtCardBazaar = (byte)(value ? 1 : 0);
    }
    [NotMapped]
    public bool HasSoldCardBazaarBool
    {
        get => HasSoldCardBazaar > 0;
        set => HasSoldCardBazaar = (byte)(value ? 1 : 0);
    }
    public DateTime OracleLastDayPlayed { get; set; }
    public string? ArenaT50Deck { get; set; }
    public int? ArenaT50Mark { get; set; }
    public int? ArenaWins { get; set; }
    public int? ArenaLosses { get; set; }
}