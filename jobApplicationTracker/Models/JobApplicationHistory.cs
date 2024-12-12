using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.Models
{
    public class JobApplicationHistory
    {
        [Key]
        public Guid Id { get; set; }
        public Guid JobApplicationId { get; set; }
        public Guid StatusId { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
