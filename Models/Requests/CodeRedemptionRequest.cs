using FastEndpoints;

namespace ElementscrAPI.Models.Requests;

public class CodeRedemptionRequest
{
    [FromHeader("Authorization")]
    public string AccessToken { get; set; }
    public string RedeemCode { get; set; }
}