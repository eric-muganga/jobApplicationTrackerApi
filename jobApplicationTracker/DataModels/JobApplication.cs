

namespace jobApplicationTrackerApi.DataModels
{
    public class JobApplication
    {
        public Guid Id { get; set; }
        public string Company { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public Guid StatusId { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string? Notes { get; set; }
        public Guid ContractTypeId { get; set; }
        public string? JobDescription { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid FinancialInformationId { get; set; }
        
        
        public Status Status { get; set; }
        public ContractType ContractType { get; set; }
        public FinancialInformation FinancialInformation { get; set; }


        public ICollection<Interview> Interviews { get; set; }
    }
}