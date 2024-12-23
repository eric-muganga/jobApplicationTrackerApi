﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using jobApplicationTrackerApi.Services;
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.ViewModels;
using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;
using Azure;

namespace jobApplicationTrackerApi.Controllers;


[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class JobApplicationController(IJobApplicationService jobApplicationService) : Controller 
{
    /// <summary>
    /// Retrieves a list of all job applications.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllJobApplications()
    {
        var jobApplications =  await jobApplicationService.GetJobApplicationsAsync();
        try
        {
            if (jobApplications.Data == Empty)           //Is it ok?
            {
                jobApplications.Message = "No instances of Job Applications were found";
                jobApplications.StatusCode = HttpStatusCode.NotFound;
                return NotFound(jobApplications);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }

        return GenerateResponse(jobApplications);    //jobApplications / jobApplications.Data ?
    }

    /// <summary>
    /// Retrieves a specific job application by its unique identifier.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetJobApplicationById(Guid jobId)
    {
        var jobApplication = await jobApplicationService.GetJobApplicationByGuidAsync(jobId);
        return GenerateResponse(jobApplication);
    }

    /// <summary>
    /// Adds a new job application and returns the created entity.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateJobApplication([FromBody] JobApplication jobApplication)
    {
        var jobAppCreated = new ServiceResponse<JobApplication>();
        try
        {
            jobAppCreated = await jobApplicationService.AddJobApplicationAsync(jobApplication);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return GenerateResponse(jobAppCreated);
    }

    /// <summary>
    /// Updates an existing job application and returns the updated entity.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateJobApplication([FromBody] JobApplication jobApplication)
    {
        try
        {
            await jobApplicationService.UpdateJobApplicationAsync(jobApplication);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(jobApplication);
    }

    /// <summary>
    /// Deletes a job application by its unique identifier.
    /// Returns a simple response without a data payload.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteJobApplication([FromRoute] Guid jobId)
    {
        try
        {
            var delited = await jobApplicationService.DeleteJobApplicationAsync(jobId);
            if(delited == 0)
                return NotFound($"JobApplication with id={jobId} was not found");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(ServiceResponse);
    }

    /// <summary>
    /// Update the status of a job application.
    /// </summary>
    [HttpPatch("{id:guid}/status/{statusId:guid}")]
    public async Task<IActionResult> UpdateStatus(Guid id, Guid statusId)
    {
        var response = await jobApplicationService.UpdateJobApplicationStatusAsync(id, statusId);
        return GenerateResponse(response);
    }

    /// <summary>
    /// Update the contract type of a job application.
    /// </summary>
    [HttpPatch("{id:guid}/contract-type/{contractTypeId:guid}")]
    public async Task<IActionResult> UpdateContractType(Guid id, Guid contractTypeId)
    {
        var response = await jobApplicationService.UpdateJobApplicationContractTypeAsync(id, contractTypeId);
        return GenerateResponse(response);
    }

    //Helping function to manage response types 
    private IActionResult GenerateResponse<T>(ServiceResponse<T> response)
    {
        if (!response.Success)
            return StatusCode(Convert.ToInt32(response.StatusCode), response);

        if (response.StatusCode.Equals(HttpStatusCode.Created))
            return StatusCode(Convert.ToInt32(response.StatusCode), response);

        return Ok(response);
    }

}