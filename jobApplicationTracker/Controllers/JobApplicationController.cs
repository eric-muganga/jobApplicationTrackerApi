using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using jobApplicationTrackerApi.Services;
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.ViewModels;

namespace jobApplicationTrackerApi.Controllers;


[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class JobApplicationController(IJobApplicationService jobApplicationService) : Controller 
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var jobApplications =  await jobApplicationService.GetJobApplicationsAsync();
        if(jobApplications.Data == Empty)           //Is it ok?
        {
            //var serviceResponse = new ServiceResponse<IEnumerable<JobApplication>>();
            jobApplications.Message = "No instances of Job Applications were found";
            jobApplications.StatusCode = HttpStatusCode.NotFound;
            return NotFound(jobApplications);
        }

        return Ok(jobApplications.Data);    //jobApplications / jobApplications.Data ?
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var jobApplication = await jobApplicationService.GetJobApplicationByGuidAsync(id);
        //return GenerateResponse(response);
    }

}


/*
 *     public T Data { get; set; }
    public bool Success { get; set; } = true;
    public string? Message { get; set; } = null;
    public List<string>? ErrorMessages { get; set; } = null;
    public Enum StatusCode { get; set; }
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