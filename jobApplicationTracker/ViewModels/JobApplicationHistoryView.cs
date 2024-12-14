namespace jobApplicationTrackerApi.ViewModels
{
    public class JobApplicationHistoryView
    {
        public Guid Id { get; set; }
        public Guid JobApplicationId { get; set; }
        public string StatusName { get; set; }      //new property
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
