namespace ElementscrAPI.Entities;

public class AdminUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string AccountId { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
}