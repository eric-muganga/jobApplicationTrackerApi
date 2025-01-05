namespace jobApplicationTrackerApi.Controllers;

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


[Route("api/[controller]")]
[ApiController]
//[Authorize]

public class JobApplicationHistoryController(IJobApplicationHistoryService historyService, IMapper mapper) : JobAppControllerBase
{
    /// <summary>
    /// Gets the history of status changes for a specific job application.
    /// </summary>
    [HttpGet("{jobApplicationId:guid}/history")]
    public async Task<IActionResult> GetHistoryByJobApplicationId(Guid jobApplicationId)
    {
        var returnModel = new ServiceResponse<IEnumerable<JobApplicationHistoryView>>();
        var response = await historyService.GetHistoryByJobApplicationIdAsync(jobApplicationId);

        var viewModel = mapper.Map<IEnumerable<JobApplicationHistoryView>>(response.Data);
        returnModel.Data = (IEnumerable<JobApplicationHistoryView>)viewModel;
        returnModel.Success = response.Success;
        returnModel.Message = response.Message;
        returnModel.ErrorMessages = response.ErrorMessages;
        returnModel.StatusCode = response.StatusCode;

        return GenerateResponse(returnModel);
    }

    /// <summary>
    /// Adds a new history record for a job application.
    /// </summary>
    [HttpPost("history")]
    public async Task<IActionResult> AddHistoryRecord([FromBody] JobApplicationHistoryView history)
    {
        var returnModel = new ServiceResponse<JobApplicationHistoryView>();
        try
        {
            var jobApplicationHistory = mapper.Map<JobApplicationHistory>(history);

            var jobAppHistoryDto = await historyService.AddHistoryRecordAsync(jobApplicationHistory);

            returnModel = mapper.Map<ServiceResponse<JobApplicationHistoryView>>(jobAppHistoryDto);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(returnModel);
    }
}
