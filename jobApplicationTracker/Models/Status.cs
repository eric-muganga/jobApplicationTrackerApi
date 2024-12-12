using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }     //could be [Guid]
        [Required]
        [MaxLength(50, ErrorMessage = "Status name should not exceed 50 characters")]
        public string Name { get; set; }
        [Required]
        public int SortOrder { get; set; }
    }
}
