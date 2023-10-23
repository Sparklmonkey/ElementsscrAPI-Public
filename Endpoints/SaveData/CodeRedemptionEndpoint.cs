using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Filters;
using ElementscrAPI.Models.Requests;
using ElementscrAPI.Repositories;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.SaveData;

[HttpPut("api/save-data/redeem")]
public class CodeRedemptionEndpoint: Endpoint<CodeRedemptionRequest, CodeDetails>
{
    private readonly IJwtAuth _jwtAuth;
    private readonly PlayerDataContext _context;

    public CodeRedemptionEndpoint(IJwtAuth jwtAuth, PlayerDataContext context)
    {
        _context = context;
        _jwtAuth = jwtAuth;
    }

    public override async Task HandleAsync(CodeRedemptionRequest request, CancellationToken ct)
    {
        var accountId = _jwtAuth.GetAccountId(request.AccessToken);
        var player = await _context.PlayerData.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();
        if (player is null)
        {
            await SendOkAsync(new CodeDetails()
            {
                CodeName = "NotValid"
            }, ct);
        }

        if (player.RedeemCodes.Contains(request.RedeemCode))
        {
            await SendOkAsync(new CodeDetails()
            {
                CodeName = "NotValid"
            }, ct);
        }

        var code = await _context.RedeemCodes.Where(x => x.CodeDetails.CodeName == request.RedeemCode).FirstOrDefaultAsync();
        if (code is null)
        {
            await SendOkAsync(new CodeDetails()
            {
                CodeName = "NotValid"
            }, ct);
        }

        if (code.CodeDetails.IsSingleUse)
        {
            _context.RedeemCodes.Remove(code);
        }
        player.RedeemCodes.Add(code.CodeDetails.CodeName);
        await _context.SaveChangesAsync();
        
        await SendOkAsync(code.CodeDetails, ct);
    }
}