
namespace jobApplicationTrackerApi.DataModels
{
    public class Interview
    {
        public Guid Id { get; set; }
        public Guid JobApplicationId { get; set; }
        public string Type { get; set; }
        public DateTime? Date { get; set; }
        public string Notes { get; set; } = string.Empty;


        public JobApplication JobApplication { get; set; }
    }
}