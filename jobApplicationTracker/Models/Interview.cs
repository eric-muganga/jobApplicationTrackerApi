using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.Models
{
    public class Interview
    {
        [Key]
        public int Id { get; set; }                 //could be [Guid]
        public int JobApplicationId { get; set; }   //could be [Guid]
        [Required]
        [MaxLength(50, ErrorMessage = "Type should be less than 50 characters")]
        public string Type { get; set; }
        public DateTime? Date { get; set; }      // I made Date nullable in case you don't know the date of interview
        public string Notes { get; set; } = string.Empty;   // should it be string.Empty or string? 
    }
}