using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using jobApplicationTrackerApi.Services;
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.ViewModels;
using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;
using Azure;
using AutoMapper;

namespace jobApplicationTrackerApi.Controllers;


[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class JobApplicationController(IJobApplicationService jobApplicationService, IMapper mapper) : JobAppControllerBase
{

    /// <summary>
    /// Retrieves a list of all job applications.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllJobApplications()        //make a view model and return it?
    {
        var jobApplications =  await jobApplicationService.GetJobApplicationsAsync();// filter by current user id
        return GenerateResponse(jobApplications);

        /*        var jobApplication = new JobApplication { Id = 1, JobTitle = "Software Developer", UserId = 123 };
        */
        // Use AutoMapper to convert to JobApplicationView
        var jobApplicationsView = mapper.Map<IEnumerable<JobApplicationView>>(jobApplications);////// TODO: XXXXX


        return Ok(jobApplicationsView);
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
    public async Task<IActionResult> CreateJobApplication([FromBody] JobApplicationView jobApplicationView)
    {
        var jobAppCreated = new ServiceResponse<JobApplicationView>();
        try
        {
            var jobApplication = mapper.Map<JobApplication>(jobApplicationView);
            //jobApplication.UserId = userId;                                           //Uncomment

            //JobApplication jobApplication = new();
            // map here see automapper
            // add id
            var jobAppCreatedDto = await jobApplicationService.AddJobApplicationAsync(jobApplication);
            // reverse map or
            // jobApplicationView.Id = jobAppCreatedDto.Id


            
        //ex data - view (in controller)
        //var jobApplication = new JobApplication { Id = 1, JobTitle = "Software Developer", UserId = 123 };
        //var jobApplicationView = _mapper.Map<JobApplicationView>(jobApplication);

        //ex view - data (in controller)
        //var jobApplicationView = new JobApplicationView { Id = 1, JobTitle = "Software Developer" };
        //var jobApplication = _mapper.Map<JobApplication>(jobApplicationView);
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
            //var delited = await jobApplicationService.DeleteJobApplicationAsync(jobId);
            //if(delited == 0)
            //    return NotFound($"JobApplication with id={jobId} was not found");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
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

}