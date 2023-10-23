using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Filters;
using ElementscrAPI.Helpers;
using ElementscrAPI.Models;
using ElementscrAPI.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly PlayerDataContext _playerDataContext;
    private readonly IJwtAuth _jwtAuth;

    public AdminRepository(PlayerDataContext playerDataContext, IJwtAuth jwtAuth)
    {
        _playerDataContext = playerDataContext;
        _jwtAuth = jwtAuth;
    }

    public async Task<int> TestMigrate()
    {
        var list = await _playerDataContext.PlayerData.ToListAsync();
        return list.Count;
        // var migrationList = new List<PlayerData>();
        // var oldPlayerList = await _playerDataContext.UserData.Where(x => true).ToListAsync();
        // // return oldPlayerList;
        // oldPlayerList.RemoveAll(x => false);
        // var oldSaveList = await _playerDataContext.SavedData.ToListAsync();
        // var indexId = 2;
        // foreach (var user in oldPlayerList)
        // {
        //     var id = int.Parse(user.SavedDataId);
        //     var save = oldSaveList.Find(x => x.Id == id);
        //     if (save is null)
        //     {
        //         continue;
        //     }
        //     var player = new PlayerData()
        //     {
        //         Id = indexId,
        //         GameStats = new GameStats(),
        //         RedeemCodes = new List<string>(),
        //         AccessToken = "NotValid",
        //         AccountId = Guid.NewGuid().ToString(),
        //         UserData = new UserData()
        //         {
        //             Username = user.Username,
        //             Password = user.UserPassKey,
        //             EmailAddress = user.EmailAddress,
        //             LastLogin = DateTime.UtcNow,
        //             Salt = user.Salt
        //         },
        //         SavedData = new()
        //         {
        //             MarkElement = save.MarkElementInt,
        //             ArenaT50Deck = save.ArenaT50Deck?.Split(" ")
        //                 .ToList() ?? new List<string>(),
        //             ArenaLosses = save.ArenaLosses ?? 0,
        //             ArenaT50Mark = save.ArenaT50Mark ?? 0,
        //             ArenaWins = save.ArenaWins ?? 0,
        //             CurrentDeck = save.DeckCards.Split(" ")
        //                 .ToList(),
        //             InventoryCards = save.InventoryCards.Split(" ").ToList(),
        //             CurrentQuestIndex = save.CurrentQuestIndexInt,
        //             PlayedOracleToday = save.PlayedOracleTodayBool,
        //             Electrum = Convert.ToInt32(save.Electrum),
        //             GamesLost = Convert.ToInt32(save.GamesLost),
        //             PlayerScore = Convert.ToInt32(save.PlayerScore),
        //             GamesWon = Convert.ToInt32(save.GamesWon),
        //             HasBoughtCardBazaar = save.HasBoughtCardBazaarBool,
        //             HasSoldCardBazaar = save.HasSoldCardBazaarBool,
        //             OracleLastPlayed = save.OracleLastDayPlayed,
        //             HasDefeatedLevel1 = save.HasDefeatedLevel1Bool,
        //             HasDefeatedLevel0 = save.HasDefeatedLevel0Bool,
        //             HasDefeatedLevel2 = save.HasDefeatedLevel2Bool,
        //             RemovedCardFromDeck = save.RemovedCardFromDeckBool
        //         }
        //     };
        //     migrationList.Add(player);
        //     indexId++;
        // }
        // // var user = await _playerDataContext.UserData.Where(x => x.Username == "TheGreat").FirstOrDefaultAsync();
        // await _playerDataContext.PlayerData.AddRangeAsync(migrationList);
        // await _playerDataContext.SaveChangesAsync();
        // return true;
    }

    public async Task<string> RegisterAdmin(LoginRequest loginRequest)
    {
        var admin = await _playerDataContext.AdminUser.Where(x => x.AccountId == loginRequest.Username)
            .FirstOrDefaultAsync();
        if (admin is null)
        {
            return "Failed";
        }

        var salt = ExtendMethods.GenerateRndSalt();
        var hashPass = loginRequest.Password.EncryptPassword(salt);
        admin.Salt = salt;
        admin.Password = hashPass;
        await _playerDataContext.SaveChangesAsync();
        return "Success";
    }

    public async Task<string> LoginAdmin(LoginRequest loginRequest)
    {
        var admin = await _playerDataContext.AdminUser.Where(x => x.Username == loginRequest.Username)
            .FirstOrDefaultAsync();
        if (admin is null)
        {
            return "Failed";
        }

        var hashPass = loginRequest.Password.EncryptPassword(admin.Salt);
        if (admin.Password != hashPass)
        {
            return "Failed";
        }

        var token = _jwtAuth.Authentication(admin.Username, admin.AccountId, "yes");
        return token;
    }

    public async Task<bool> AddNewCode(CodeDetails codeDetails, string token)
    {
        var accountId = _jwtAuth.GetAccountId(token);
        var admin = await _playerDataContext.AdminUser.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
        if (admin is null)
        {
            return false;
        }

        _playerDataContext.RedeemCodes.Add(new RedeemCodes()
        {
            CodeDetails = codeDetails,
            Id = _playerDataContext.RedeemCodes.ToArray().Length + 1
        });
        await _playerDataContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ResetPlayerPassword(PlayerPasswordResetRequest resetRequest, string token)
    {
        var accountId = _jwtAuth.GetAccountId(token);
        var admin = await _playerDataContext.AdminUser.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
        if (admin is null)
        {
            return false;
        }

        var player = await _playerDataContext.PlayerData.Where(x => x.UserData.Username == resetRequest.Username)
            .FirstOrDefaultAsync();
        if (player is null)
        {
            return false;
        }

        player.UserData.Password = resetRequest.NewPassword.EncryptPassword(player.UserData.Salt);

        await _playerDataContext.SaveChangesAsync();
        return true;
    }
}