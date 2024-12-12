using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.Models
{
    public class JobApplication
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100, ErrorMessage = "Company name could not exceed 100 characters")]
        public string Company { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Job Title could not exceed 100 characters")]
        public string JobTitle { get; set; } = string.Empty;
        [Required]
        public Guid StatusId { get; set; }
        public DateTime? ApplicationDate { get; set; }   //class diagram want it to be not null, but if we are planning to create a canban board if it will be in a so called TODO table it should be null in my opinion. 
        public DateTime? InterviewDate { get; set; }
        public string? Notes { get; set; }
        public decimal? Salary { get; set; }
        public Guid EmploymentTypeId { get; set; }
        public string? JobDescription { get; set; }
        [Required]
        public string UserId { get; set; }          //As I remember Identity framework user has id of string tyep
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}