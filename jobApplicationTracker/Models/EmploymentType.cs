using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.Models
{
    public class EmploymentType
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
