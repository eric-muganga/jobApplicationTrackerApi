using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.DataModels
{
    public class JobApplicationHistory
    {
        public Guid Id { get; set; }
        public Guid JobApplicationId { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Guid StatusId { get; set; }

        public Status Status { get; set; }
        public JobApplication JobApplication { get; set; }
    }
}