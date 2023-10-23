namespace ElementscrAPI.Entities;

public class UserData
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string EmailAddress { get; set; }
    public DateTime LastLogin { get; set; }
}