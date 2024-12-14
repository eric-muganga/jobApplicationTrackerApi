namespace jobApplicationTrackerApi.ViewModels
{
    public class JobApplicationView
    {
        public Guid Id { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public Guid StatusId { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string Notes { get; set; }
        public Guid ContractTypeId { get; set; }
        public string? JobDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid FinancialInformationId { get; set; }    //public FinancialInformationViewModel FinancialInformation { get; set; }

    }
}
