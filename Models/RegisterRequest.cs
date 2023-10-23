using ElementscrAPI.Entities;

namespace ElementscrAPI.Models;

public class RegisterRequest
{
    
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public SavedData DataToLink { get; set; }
}