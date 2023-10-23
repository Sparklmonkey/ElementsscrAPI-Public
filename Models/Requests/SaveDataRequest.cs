using ElementscrAPI.Entities;
using FastEndpoints;

namespace ElementscrAPI.Models.Requests;

public class SaveDataRequest
{
    [FromHeader("Authorization")]
    public string AccessToken { get; set; }
    public SavedData SavedData { get; set; }
}