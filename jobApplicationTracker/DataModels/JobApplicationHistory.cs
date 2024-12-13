using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.Models
{
    public class JobApplicationHistory
    {
        public Guid Id { get; set; }
        public Guid JobApplicationId { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        public JobApplication JobApplication { get; set; }


        public ICollection<JobApplication> JobApplications { get; set; }
    }
}