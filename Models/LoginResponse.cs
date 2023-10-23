using ElementscrAPI.Entities;

namespace ElementscrAPI.Models;

public class LoginResponse
{
    public string Username { get; set; }
    public string EmailAddress { get; set; }
    public SavedData SavedData { get; set; }
    public ErrorCases ErrorMessage { get; set; }
    public string Token { get; set; }
    public string AccessToken { get; set; }
}