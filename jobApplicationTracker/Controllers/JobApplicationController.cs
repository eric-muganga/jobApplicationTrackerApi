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
using Microsoft.AspNetCore.Identity;

namespace jobApplicationTrackerApi.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class JobApplicationController(IJobApplicationService jobApplicationService, IMapper mapper) : JobAppControllerBase
{

    /// <summary>
    /// Retrieves a list of all job applications.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllJobApplications()
    {
        var returnModel = new ServiceResponse<IEnumerable<JobApplicationView>>();
        var jobApplications =  await jobApplicationService.GetJobApplicationsAsync(userId);

        var viewModel = mapper.Map<IEnumerable<JobApplicationView>>(jobApplications.Data);
        returnModel.Data = (IEnumerable<JobApplicationView>)viewModel;
        returnModel.Success = jobApplications.Success;
        returnModel.Message = jobApplications.Message;
        returnModel.ErrorMessages = jobApplications.ErrorMessages;
        returnModel.StatusCode = jobApplications.StatusCode;

        return GenerateResponse(returnModel);
    }

    /// <summary>
    /// Retrieves a specific job application by its unique identifier.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetJobApplicationById(Guid id)
    {
        var returnModel = new ServiceResponse<JobApplicationView>();

        var jobApplication = await jobApplicationService.GetJobApplicationByGuidAsync(id, userId);
        returnModel = mapper.Map<ServiceResponse<JobApplicationView>>(jobApplication);

        return GenerateResponse(returnModel);
    }

    /// <summary>
    /// Adds a new job application and returns the created entity.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateJobApplication([FromBody] JobApplicationView jobApplicationView)
    {
        var returnModel = new ServiceResponse<JobApplicationView>();
        try
        {
            var jobApplication = mapper.Map<JobApplication>(jobApplicationView);
            jobApplication.UserId = Guid.Parse(userId);

            var jobAppCreatedDto = await jobApplicationService.AddJobApplicationAsync(jobApplication);

            returnModel = mapper.Map<ServiceResponse<JobApplicationView>>(jobAppCreatedDto);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(returnModel);
    }

    /// <summary>
    /// Updates an existing job application and returns the updated entity.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateJobApplication([FromBody] JobApplicationView jobApplicationView)
    {
        var returnModel = new ServiceResponse<JobApplicationView>();
        try
        {
            var jobApplication = mapper.Map<JobApplication>(jobApplicationView);
            jobApplication.UserId = Guid.Parse(userId);

            var jobAppUpdateDto = await jobApplicationService.UpdateJobApplicationAsync(jobApplication, userId);

            returnModel = mapper.Map<ServiceResponse<JobApplicationView>>(jobAppUpdateDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(returnModel);
    }

    /// <summary>
    /// Deletes a job application by its unique identifier.
    /// Returns a simple response without a data payload.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteJobApplication([FromRoute] Guid id)
    {
        var returnModel = new ServiceResponse<JobApplicationView>();

        try
        {
            var jobAppDeleteDto = await jobApplicationService.DeleteJobApplicationByIdAsync(id, userId);
            returnModel = mapper.Map<ServiceResponse<JobApplicationView>>(jobAppDeleteDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(returnModel);
    }

    /// <summary>
    /// Update the status of a job application.
    /// </summary>
    [HttpPatch("{id:guid}/status/{statusId:guid}")]
    public async Task<IActionResult> UpdateStatus(Guid id, Guid statusId)
    {
        var returnModel = new ServiceResponse<JobApplicationView>();

        try
        {
            var jobAppUpdateStatusDto = await jobApplicationService.UpdateJobApplicationStatusAsync(id, statusId, userId);
            returnModel = mapper.Map<ServiceResponse<JobApplicationView>>(jobAppUpdateStatusDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(returnModel);
    }

    /// <summary>
    /// Update the contract type of a job application.
    /// </summary>
    [HttpPatch("{id:guid}/contract-type/{contractTypeId:guid}")]
    public async Task<IActionResult> UpdateContractType(Guid id, Guid contractTypeId)
    {
        var returnModel = new ServiceResponse<JobApplicationView>();

        try
        {
            var jobAppUpdateContractTypeDto = await jobApplicationService.UpdateJobApplicationContractTypeAsync(id, contractTypeId, userId);
            returnModel = mapper.Map<ServiceResponse<JobApplicationView>>(jobAppUpdateContractTypeDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(returnModel);
    }

}