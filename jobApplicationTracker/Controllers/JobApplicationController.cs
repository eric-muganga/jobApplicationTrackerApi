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
    /// Retrieves a number of job applications for the past three months.
    /// </summary>
    [HttpGet("statistics-per-months")]
    public async Task<IActionResult> GetJobApplicationsForThreeMonths()
    {
        var result = await jobApplicationService.GetJobApplicationsPerMonthsAsync(userId);
        //maybe call both functions from one method and then wait until both methods return some result using (await)

        if (result == null || !result.Data.Any())
        {
            return BadRequest(result);
        }
        return Ok(result);
    }


    /// <summary>
    /// Retrieves a number of job applications with different statuses.
    /// </summary>
    [HttpGet("statistics-by-statuses")]
    public async Task<IActionResult> GetJobApplicationsByStatuses()
    {
        var result = await jobApplicationService.GetJobApplicationsByStatusesAsync(userId);

        if (result == null || !result.Data.Any())
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
    //number of jobs by statuses for the whole time

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
            jobApplication.Id = Guid.NewGuid(); // generate ID if not provided
            jobApplication.UserId = Guid.Parse(userId);

            var jobAppCreatedDto = await jobApplicationService.AddJobApplicationAsync(jobApplication);
            
            // just in case It's not success, it returns an appropriate HTTP status code along with the service response.
            if (!jobAppCreatedDto.Success)
            {
                return StatusCode((int)jobAppCreatedDto.StatusCode, jobAppCreatedDto);
            }
            
            
            // 5) The service now returns a fully loaded 'JobApplication' with Status navigation
            // Map it back to the view model, which includes the Status string
            var savedEntity = jobAppCreatedDto.Data;
            var jobAppView = mapper.Map<JobApplicationView>(savedEntity);
            

            // 6) Build your ServiceResponse
            returnModel.Data = jobAppView;
            returnModel.Success = true;
            returnModel.Message = "Job application added successfully.";
            returnModel.StatusCode = HttpStatusCode.Created;
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
        try
        {
            Console.WriteLine($"Received JobApplicationId: {id}");
            Console.WriteLine($"Received StatusId: {statusId}");
            
            var serviceResponse = await jobApplicationService.UpdateJobApplicationStatusAsync(id, statusId, userId);
            
            if (!serviceResponse.Success)
            {
                return StatusCode((int)serviceResponse.StatusCode, serviceResponse);
            }

            //Map the domain model (serviceResponse.Data) -> JobApplicationView
            var updatedView = mapper.Map<JobApplicationView>(serviceResponse.Data);

            //Wrap it in a new ServiceResponse<JobApplicationView> 
            var returnModel = new ServiceResponse<JobApplicationView>
            {
                Data = updatedView,
                Success = true,
                Message = serviceResponse.Message,
                ErrorMessages = serviceResponse.ErrorMessages,
                StatusCode = serviceResponse.StatusCode
            };

            // Return 200 OK
            return Ok(returnModel);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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