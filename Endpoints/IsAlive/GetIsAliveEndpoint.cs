using ElementscrAPI.Data;
using ElementscrAPI.Entities;
using ElementscrAPI.Repositories;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ElementscrAPI.Endpoints.IsAlive;

[HttpGet("api/is-alive"), AllowAnonymous]
public class GetIsAliveEndpoint : EndpointWithoutRequest<bool>
{
    private readonly PlayerDataContext _context;

    public GetIsAliveEndpoint(PlayerDataContext context)
    {
        _context = context;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(true);
    }
}