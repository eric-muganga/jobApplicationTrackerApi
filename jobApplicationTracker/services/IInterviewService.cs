using jobApplicationTrackerApi.DataModels;

namespace jobApplicationTrackerApi.Services;

public interface IInterviewService
{
    /// <summary>
    /// Retrieves all interviews for a specific job application by its ID.
    /// </summary>
    Task<ServiceResponse<IEnumerable<Interview>>> GetInterviewsByJobApplicationIdAsync(Guid jobApplicationId);
    
    /// <summary>
    /// Retrieves all interviews for a specific job application by its ID.
    /// </summary>
    Task<ServiceResponse<Interview>> AddInterviewAsync(Interview interview);
    
    /// <summary>
    /// Updates an existing interview record.
    /// </summary>
    Task<ServiceResponse<Interview>> UpdateInterviewAsync(Interview interview);
    
    /// <summary>
    /// Deletes an interview record by its ID.
    /// </summary>
    Task<ServiceResponse<bool>> DeleteInterviewAsync(Guid interviewId);
    
    /// <summary>
    /// Exports all interviews for a job application as an ICS calendar file.
    /// </summary>
    Task<ServiceResponse<byte[]>> ExportInterviewsToICSAsync(Guid jobApplicationId); // Example for .ICS export
}