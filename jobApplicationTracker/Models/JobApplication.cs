using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }    //Could be [Guid] instead of [int]

        [MaxLength(100, ErrorMessage = "Company name could not exceed 100 characters")]
        public string Company { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Job Title could not exceed 100 characters")]
        public string JobTitle { get; set; } = string.Empty;
        [Required]
        public int StatusId { get; set; }  //could be [Guid]
        public DateTime? ApplicationDate { get; set; }   //class diagram want it to be not null, but if we are planning to create a canban board if it will be in a so called TODO table it should be null in my opinion. 
        public DateTime? InterviewDate { get; set; }
        public string? Notes { get; set; }
        public decimal? Salary { get; set; }
        public int EmploymentTypeId { get; set; }   //could be [Guid]
        public string? JobDescription { get; set; }
        [Required]
        public int UserId { get; set; }             //could be [Guid]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}