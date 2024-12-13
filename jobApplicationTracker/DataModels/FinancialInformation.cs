namespace jobApplicationTrackerApi.DataModels
{
    public class FinancialInformation
    {
        public Guid Id { get; set; }
        public string Salary { get; set; }
        public string Currency { get; set; }
        public string SalaryType { get; set; }    //Add in the future table CurencyTypeId with currency types
        public string TypeOfEmployment { get; set; }
    }
}