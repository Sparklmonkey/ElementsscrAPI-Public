using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Models;
using ElementscrAPI.Models.Requests;

namespace ElementscrAPI.Repositories;

public interface IAdminRepository
{
    Task<string> RegisterAdmin(LoginRequest registerRequest);
    Task<int> TestMigrate();
    Task<string> LoginAdmin(LoginRequest loginRequest);
    Task<bool> AddNewCode(CodeDetails loginRequest, string token);
    Task<bool> ResetPlayerPassword(PlayerPasswordResetRequest resetRequest, string token);
}