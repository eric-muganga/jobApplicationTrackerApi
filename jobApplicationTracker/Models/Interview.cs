using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.Models
{
    public class Interview
    {
        [Key]
        public Guid Id { get; set; }
        public Guid JobApplicationId { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Type should be less than 50 characters")]
        public string Type { get; set; }
        public DateTime? Date { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}