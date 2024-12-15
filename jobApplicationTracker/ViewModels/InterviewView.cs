namespace jobApplicationTrackerApi.ViewModels
{
    public class InterviewView
    {
        public Guid Id { get; set; }
        public Guid JobApplicationId { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
