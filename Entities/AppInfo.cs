namespace ElementscrAPI.Entities;

public class AppInfo
{
    public int Id { get; set; }
    public bool IsMaintenance { get; set; }
    public bool ShouldUpdate { get; set; }
    public string UpdateNote { get; set; }
}

public class PreviousUpdates
{
    public string AppVersion { get; set; }
    public string UpdateNote { get; set; }
}