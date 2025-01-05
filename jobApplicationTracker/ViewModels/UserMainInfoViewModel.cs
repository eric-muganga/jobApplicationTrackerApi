namespace jobApplicationTrackerApi.ViewModels;

public class UserMainInfoViewModel
{
    public Guid Id { get; set; }

    public RoleType RoleId { get; set; }

    public string? Email { get; set; }
}

public enum RoleType : byte
{
    NoRole = 0,
    Administrator
}