using Microsoft.AspNetCore.Identity;

namespace jobApplicationTrackerApi.DataModels;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    
    public string FullName { get; set; }
    
    public ICollection<JobApplication> JobApplications { get; set; }
}