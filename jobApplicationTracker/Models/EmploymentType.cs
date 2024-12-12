using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.Models
{
    public class EmploymentType
    {
        [Key]
        public int Id { get; set; }     //could be [Guid]
        [Required]
        public string Name { get; set; }
    }
}
