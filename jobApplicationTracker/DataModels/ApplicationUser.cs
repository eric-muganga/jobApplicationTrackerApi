using Microsoft.AspNetCore.Identity;

namespace jobApplicationTrackerApi.DataModels;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    
    public string FullName { get; set; }
}