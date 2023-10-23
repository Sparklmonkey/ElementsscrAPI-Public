namespace ElementscrAPI.Filters
{
    public interface IJwtAuth
    {
        string Authentication(string username, string accountId, string isAdmin);
        string AccessTokenCreation(string username, string accountId);
        string GetAccountId(string jwtToken);
        string RefreshToken(string jwtToken);
    }
}
