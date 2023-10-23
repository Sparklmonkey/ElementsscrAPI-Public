
using FastEndpoints;

namespace ElementscrAPI.Models;

public class GameStatRequest
{
    [FromHeader("Authorization")]
    public string AccessToken { get; set; }
    public int AiLevel { get; set; }
    public string AiName { get; set; }
    public bool IsWin { get; set; }
}