using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.Models
{
    public class JobApplicationHistory
    {
        [Key]
        public int Id { get; set; }     //could be [Guid]
        public int JobApplicationId { get; set; }   //could be [Guid]
        public int StatusId { get; set; }   //could be [Guid]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
