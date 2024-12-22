using jobApplicationTrackerApi.DataModels;

namespace jobApplicationTrackerApi.Services;

public interface IJobApplicationHistoryService
{
    /// <summary>
    /// Retrieves the history of status changes for a specific job application.
    /// </summary>
    Task<ServiceResponse<IEnumerable<JobApplicationHistory>>> GetHistoryByJobApplicationIdAsync(Guid jobApplicationId);

    /// <summary>
    /// Adds a new history record for a status change.
    /// </summary>
    Task<ServiceResponse<JobApplicationHistory>> AddHistoryRecordAsync(JobApplicationHistory history);
    
}