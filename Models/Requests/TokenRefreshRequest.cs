using FastEndpoints;

namespace ElementscrAPI.Models.Requests;

public class TokenRefreshRequest
{
    public string AccessToken { get; set; }
}