using ElementscrAPI.Endpoints.Login;
using FastEndpoints;

namespace ElementscrAPI.Summary;

public class LoginCredentialSummary: Summary<LoginCredentialsEndpoint>
{
    public LoginCredentialSummary()
    {
        Summary = "Login with Username and Password";
        Description = "Login with Username and Password";
        Response<bool>(200, "Successfully logged in");
        Response(404, "User not found");
    }
}