using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using jobApplicationTrackerApi.services;
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.ViewModels;

namespace jobApplicationTrackerApi.Controllers;


[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class JobApplicationController(IJobApplicationService jobApplicationService) : Controller 
{
    [HttpGet("")]       //Post Put Delete
    //[ProducesResponseType(200)]
    public IActionResult Get()
    {
        var jobApplications = jobApplicationService.GetJobApplicationsAsync();
        if(jobApplications == null)
        {
            return NotFound();
        }
        return Ok(jobApplications);
    }   //Returns status code with a list of all job applications.

}


/*
 {
    /// <summary>
    /// Retrieves a list of all job applications.
    /// </summary>
    Task<ServiceResponse<IEnumerable<JobApplication>>> GetJobApplicationsAsync();
    
    /// <summary>
    /// Retrieves a specific job application by its unique identifier.
    /// </summary>
    Task<ServiceResponse<JobApplication>> GetJobApplicationByGuidAsync(Guid jobId);
    
    /// <summary>
    /// Adds a new job application and returns the created entity.
    /// </summary>
    Task<ServiceResponse<JobApplication>> AddJobApplicationAsync(JobApplication jobApplication);
    
    /// <summary>
    /// Updates an existing job application and returns the updated entity.
    /// </summary>
    Task<ServiceResponse<JobApplication>> UpdateJobApplicationAsync(JobApplication jobApplication);
    
    /// <summary>
    /// Deletes a job application by its unique identifier.
    /// Returns a simple response without a data payload.
    /// </summary>
    Task<ServiceResponse<JobApplication>> DeleteJobApplicationAsync(JobApplication jobApplication);

    /// <summary>
    /// Updates the status of a job application based on a given statusId.
    /// Could be part of a simple workflow.
    /// </summary>
    Task<ServiceResponse<JobApplication>> UpdateJobApplicationStatusAsync(Guid jobApplicationId,
        Guid statusId);

    /// <summary>
    /// Associates an employment type (ContractType) with the job application
    /// </summary>
    Task<ServiceResponse<JobApplication>> UpdateJobApplicationContractTypeAsync(Guid jobApplicationId,
        Guid contractTypeId);
}
 */