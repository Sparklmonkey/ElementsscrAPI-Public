using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Filters;
using ElementscrAPI.Helpers;
using ElementscrAPI.Models.Requests;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.UserData;

[HttpPut("api/user-data/update")]
public class UpdateUserDataEndpoint: Endpoint<UserDataRequest, ErrorCases>
{
    private readonly IJwtAuth _jwtAuth;
    private readonly PlayerDataContext _context;

    public UpdateUserDataEndpoint(IJwtAuth jwtAuth, PlayerDataContext context)
    {
        _context = context;
        _jwtAuth = jwtAuth;
    }

    public override async Task HandleAsync(UserDataRequest request, CancellationToken ct)
    {
        var accountId = _jwtAuth.GetAccountId(request.AccessToken.Replace("Bearer ", ""));
        var player = await _context.PlayerData.Where(x => x.AccountId == accountId).FirstOrDefaultAsync();

        if (player is null)
        {
            await SendOkAsync(ErrorCases.UserMismatch);
        }

        if (request.NewUsername != "")
        {
            if (!request.NewUsername.UsernameCheck())
            {
                await SendOkAsync(ErrorCases.UsernameInvalid);
            }
            if (await _context.PlayerData.Where(x => x.UserData.Username == request.NewUsername).FirstOrDefaultAsync() is not null)
            {
                await SendOkAsync(ErrorCases.UserNameInUse);
            }

            player.UserData.Username = request.NewUsername;
        }

        if (request.NewPassword != "")
        {
            var hashPass = request.Password.EncryptPassword(player.UserData.Salt);
            if (hashPass != player.UserData.Password)
            {
                await SendOkAsync(ErrorCases.IncorrectCredentials);
            }

            if (!request.NewPassword.PasswordCheck())
            {
                await SendOkAsync(ErrorCases.PasswordInvalid);
            }

            player.UserData.Password = request.NewPassword.EncryptPassword(player.UserData.Salt);
        }

        await _context.SaveChangesAsync();
        await SendOkAsync(ErrorCases.AllGood);
    }
}