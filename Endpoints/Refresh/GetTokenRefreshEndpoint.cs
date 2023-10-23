using ElementscrAPI.Filters;
using ElementscrAPI.Models.Requests;
using ElementscrAPI.Models.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace ElementscrAPI.Endpoints.Refresh;

    [HttpGet("api/refresh/{accessToken}"), AllowAnonymous]
    public class GetTokenRefreshEndpoint : Endpoint<TokenRefreshRequest, TokenRefreshResponse>
    {
        private readonly IJwtAuth _jwtAuth;

        public GetTokenRefreshEndpoint(IJwtAuth jwtAuth)
        {
            _jwtAuth = jwtAuth;
        }

        public override async Task HandleAsync(TokenRefreshRequest request, CancellationToken ct)
        {
            await SendOkAsync(new() { NewToken = _jwtAuth.RefreshToken(request.AccessToken.Replace("Bearer ", "")) },
                ct);
        }
    }