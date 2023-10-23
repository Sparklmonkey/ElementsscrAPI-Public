namespace ElementscrAPI.Models;

public class PlayerPasswordResetRequest
{
    public string Username { get; set; }
    public string NewPassword { get; set; }
}