namespace jobApplicationTrackerApi.ViewModels;

public class CreateUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string FullName { get; set; }
}