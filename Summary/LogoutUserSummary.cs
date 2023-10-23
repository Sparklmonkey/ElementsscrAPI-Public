using ElementscrAPI.Endpoints.Logout;
using FastEndpoints;

namespace ElementscrAPI.Summary;

public class LogoutUserSummary: Summary<LogoutUserEndpoint>
{
    public LogoutUserSummary()
    {
        Summary = "Logs out a user";
        Description = "Logs out a user and deletes the current Access Token";
        Response<bool>(200, "Successfully logged out");
        Response(404, "User was not logged in");
    }
}