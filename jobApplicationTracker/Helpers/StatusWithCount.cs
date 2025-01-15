namespace jobApplicationTrackerApi.Helpers;

public class StatusWithCount
{
    public Guid StatusId { get; set; }
    public string StatusName { get; set; }
    public int Total { get; set; }
}