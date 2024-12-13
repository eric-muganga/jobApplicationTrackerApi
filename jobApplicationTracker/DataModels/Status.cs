using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.Models
{
    public class Status
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }


        public ICollection<JobApplication> JobApplications { get; set; }
    }
}