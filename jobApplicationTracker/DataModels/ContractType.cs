
namespace jobApplicationTrackerApi.DataModels
{
    public class ContractType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<JobApplication> JobApplications { get; set; }
    }
}