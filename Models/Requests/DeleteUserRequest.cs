using FastEndpoints;

namespace ElementscrAPI.Models.Requests;

public class DeleteUserRequest
{
    [FromHeader("Authorization")]
    public string AuthToken { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}