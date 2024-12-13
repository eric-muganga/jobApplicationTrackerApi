using System.ComponentModel.DataAnnotations;

namespace jobApplicationTrackerApi.Models
{
    public class ContractType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<JobApplication> JobApplications { get; set; }
    }
}