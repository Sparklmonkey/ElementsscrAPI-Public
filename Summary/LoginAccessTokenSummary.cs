using ElementscrAPI.Endpoints.Login;
using FastEndpoints;

namespace ElementscrAPI.Summary;

public class LoginAccessTokenSummary: Summary<LoginAccessTokenEndpoint>
{
    public LoginAccessTokenSummary()
    {
        Summary = "Login with an Access Token";
        Description = "Login with an Access Token";
        Response<bool>(200, "Successfully logged in");
        Response(404, "Token not valid");
    }
}