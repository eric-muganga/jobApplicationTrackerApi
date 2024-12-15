namespace jobApplicationTrackerApi.DataModels
{
    public class FinancialInformation
    {
        public Guid Id { get; set; }
        public string Salary { get; set; }
        public string Currency { get; set; }
        public string SalaryType { get; set; }
        public string TypeOfEmployment { get; set; }

        public ICollection<JobApplication> JobApplications { get; set; }
    }
}