
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.Helpers;

namespace jobApplicationTrackerApi.Services;

public interface IJobApplicationService
{
    /// <summary>
    /// Retrieves a list of all job applications.
    /// </summary>
    Task<ServiceResponse<IEnumerable<JobApplication>>> GetJobApplicationsAsync(string userId);

    /// <summary>
    /// Retrieves number of job applications for current month and two months before it.
    /// </summary>
    Task<ServiceResponse<Dictionary<string, int>>> GetJobApplicationsPerMonthsAsync(string userId);

    /// <summary>
    /// Retrieves a dictionary with statuse and number of job applications with a certain status.
    /// </summary>
    Task<ServiceResponse<List<StatusWithCount>>> GetJobApplicationsByStatusesAsync(string userId);

    /// <summary>
    /// Retrieves a specific job application by its unique identifier.
    /// </summary>
    Task<ServiceResponse<JobApplication>> GetJobApplicationByGuidAsync(Guid jobId, string userId);
    
    /// <summary>
    /// Adds a new job application and returns the created entity.
    /// </summary>
    Task<ServiceResponse<JobApplication>> AddJobApplicationAsync(JobApplication jobApplication);
    
    /// <summary>
    /// Updates an existing job application and returns the updated entity.
    /// </summary>
    Task<ServiceResponse<JobApplication>> UpdateJobApplicationAsync(JobApplication jobApplication, string userId);
    
    /// <summary>
    /// Deletes a job application by its unique identifier.
    /// Returns a simple response without a data payload.
    /// </summary>
    Task<ServiceResponse<JobApplication>> DeleteJobApplicationByIdAsync(Guid jobId, string userId);

    /// <summary>
    /// Updates the status of a job application based on a given statusId.
    /// Could be part of a simple workflow.
    /// </summary>
    Task<ServiceResponse<JobApplication>> UpdateJobApplicationStatusAsync(Guid jobApplicationId,
        Guid statusId, string userId);

    /// <summary>
    /// Associates an employment type (ContractType) with the job application
    /// </summary>
    Task<ServiceResponse<JobApplication>> UpdateJobApplicationContractTypeAsync(Guid jobApplicationId,
        Guid contractTypeId, string userId);
}