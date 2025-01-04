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
//[Authorize]
public class JobApplicationController(IJobApplicationService jobApplicationService, IMapper mapper) : JobAppControllerBase
{

    /// <summary>
    /// Retrieves a list of all job applications.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllJobApplications()
    {
        var returnModel = new ServiceResponse<JobApplicationView>();
        var jobApplications =  await jobApplicationService.GetJobApplicationsAsync(userId);

        var viewModel = mapper.Map<IEnumerable<JobApplicationView>>(jobApplications.Data);
        //returnModel = mapper.Map<ServiceResponse<JobApplicationView>>(jobApplications);

        //return GenerateResponse(jobApplications);
        return Ok(viewModel);
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

        //returnModel = jobApplication;
        //jobApplication.Data = mapper.Map<JobApplication>(jobAppView);
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

            returnModel.Success = jobAppCreatedDto.Success;
            returnModel.Message = jobAppCreatedDto.Message;
            returnModel.ErrorMessages = jobAppCreatedDto.ErrorMessages;
            returnModel.StatusCode = jobAppCreatedDto.StatusCode;

            jobApplicationView.Id = jobAppCreatedDto.Data.Id;   //or reverse map
            returnModel.Data = jobApplicationView;

            /*
        var jobApplications =  await jobApplicationService.GetJobApplicationsAsync(userId);

        var viewModel = mapper.Map<IEnumerable<JobApplicationView>>(jobApplications.Data);
        //returnModel = mapper.Map<ServiceResponse<JobApplicationView>>(jobApplications);
*/

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
    public async Task<IActionResult> UpdateJobApplication([FromBody] JobApplication jobApplication)
    {
        try
        {
            await jobApplicationService.UpdateJobApplicationAsync(jobApplication, userId);
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
    public async Task<IActionResult> DeleteJobApplication([FromRoute] Guid id)
    {
        var response = new ServiceResponse<JobApplication>();
        try
        {
            response = await jobApplicationService.DeleteJobApplicationByIdAsync(id, userId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(response);
    }

    /// <summary>
    /// Update the status of a job application.
    /// </summary>
    [HttpPatch("{id:guid}/status/{statusId:guid}")]
    public async Task<IActionResult> UpdateStatus(Guid id, Guid statusId)
    {
        var response = await jobApplicationService.UpdateJobApplicationStatusAsync(id, statusId, userId);
        return GenerateResponse(response);
    }

    /// <summary>
    /// Update the contract type of a job application.
    /// </summary>
    [HttpPatch("{id:guid}/contract-type/{contractTypeId:guid}")]
    public async Task<IActionResult> UpdateContractType(Guid id, Guid contractTypeId)
    {
        var response = await jobApplicationService.UpdateJobApplicationContractTypeAsync(id, contractTypeId, userId);
        return GenerateResponse(response);
    }

}