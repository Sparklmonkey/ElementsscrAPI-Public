using FastEndpoints;

namespace ElementscrAPI.Models;

public class LogoutRequest
{
    [FromHeader("Authorization")]
    public string AccessToken { get; set; }
}