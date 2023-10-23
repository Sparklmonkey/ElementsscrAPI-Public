using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Repositories;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.Login;

[HttpGet("api/login/app-info"), AllowAnonymous]
public class GetAppInfoEndpoint : EndpointWithoutRequest<AppInfo>
{
    private readonly PlayerDataContext _context;

    public GetAppInfoEndpoint(PlayerDataContext context)
    {
        _context = context;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var appInfo = await _context.AppInfo.Where(x => x.Id == 1).FirstOrDefaultAsync();
        await SendOkAsync(appInfo, ct);
    }
}