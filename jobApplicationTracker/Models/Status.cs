using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.Models
{
    public class Status
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Status name should not exceed 50 characters")]
        public string Name { get; set; }
        [Required]
        public int SortOrder { get; set; }
    }
}
