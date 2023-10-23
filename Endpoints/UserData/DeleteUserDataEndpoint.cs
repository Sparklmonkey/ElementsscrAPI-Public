using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Filters;
using ElementscrAPI.Helpers;
using ElementscrAPI.Models.Requests;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.UserData;

[HttpDelete("api/user-data/delete")]
public class DeleteUserDataEndpoint: Endpoint<DeleteUserRequest, ErrorCases>
{
    private readonly IJwtAuth _jwtAuth;
    private readonly PlayerDataContext _context;

    public DeleteUserDataEndpoint(IJwtAuth jwtAuth, PlayerDataContext context)
    {
        _context = context;
        _jwtAuth = jwtAuth;
    }

    public override async Task HandleAsync(DeleteUserRequest request, CancellationToken ct)
    {
        var accountId = _jwtAuth.GetAccountId(request.AuthToken.Replace("Bearer ", ""));
        var player = await _context.PlayerData.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();

        if (player is null)
        {
            await SendOkAsync(ErrorCases.UserMismatch);
        }

        if (player.UserData.Username == request.Username &&
            player.UserData.Password == request.Password.EncryptPassword(player.UserData.Salt))
        {
            _context.PlayerData.Remove(player);
            await _context.SaveChangesAsync();
            await SendOkAsync(ErrorCases.AllGood);
        }
        await SendOkAsync(ErrorCases.IncorrectCredentials);
    }
}