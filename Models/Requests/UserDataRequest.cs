using FastEndpoints;

namespace ElementscrAPI.Entities;

public class UserDataRequest
{
    [FromHeader("Authorization")]
    public string AccessToken { get; set; }
    public string Username { get; set; }
    public string NewUsername  { get; set; }
    public string Password { get; set; }
    public string NewPassword { get; set; }
}