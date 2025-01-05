namespace jobApplicationTrackerApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using jobApplicationTrackerApi.Services;
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.ViewModels;
using AutoMapper;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class InterviewController(IInterviewService interviewService, IMapper mapper) : JobAppControllerBase
{
    /// <summary>
    /// Retrieves all interviews for a specific job application by its unique identifier.
    /// </summary>
    [HttpGet("job-application/{jobApplicationId:guid}")]
    public async Task<IActionResult> GetInterviewsByJobApplicationId(Guid jobApplicationId)
    {
        var returnModel = new ServiceResponse<IEnumerable<InterviewView>>();
        var interviewsResponse = await interviewService.GetInterviewsByJobApplicationIdAsync(jobApplicationId);

        var viewModel = mapper.Map<IEnumerable<InterviewView>>(interviewsResponse.Data);
        returnModel.Data = viewModel;       //(IEnumerable<InterviewView>)viewModel;
        returnModel.Success = interviewsResponse.Success;
        returnModel.Message = interviewsResponse.Message;
        returnModel.StatusCode = interviewsResponse.StatusCode;

        return Ok(returnModel);
    }

    /// <summary>
    /// Adds a new interview record.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddInterview([FromBody] InterviewView interviewView)
    {
        var returnModel = new ServiceResponse<InterviewView>();
        try
        {
            var interview = mapper.Map<Interview>(interviewView);
            var interviewResponse = await interviewService.AddInterviewAsync(interview);

            returnModel = mapper.Map<ServiceResponse<InterviewView>>(interviewResponse);    //
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return GenerateResponse(returnModel);
    }

    /// <summary>
    /// Updates an existing interview record.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateInterview([FromBody] InterviewView interviewView)
    {
        var returnModel = new ServiceResponse<InterviewView>();
        try
        {
            var interview = mapper.Map<Interview>(interviewView);
            var interviewResponse = await interviewService.UpdateInterviewAsync(interview);

            returnModel = mapper.Map<ServiceResponse<InterviewView>>(interviewResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return GenerateResponse(returnModel);
    }

    /// <summary>
    /// Deletes an interview record by its unique identifier.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteInterview(Guid id)
    {
        var returnModel = new ServiceResponse<bool>();

        try
        {
            var interviewResponse = await interviewService.DeleteInterviewAsync(id);
            returnModel = mapper.Map<ServiceResponse<bool>>(interviewResponse); //Not necessary?
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return GenerateResponse(returnModel);
    }

    /// <summary>
    /// Exports all interviews for a job application as an ICS calendar file.
    /// </summary>
    [HttpGet("export/ics/{jobApplicationId:guid}")]
    public async Task<IActionResult> ExportInterviewsToICS(Guid jobApplicationId)
    {
        var returnModel = new ServiceResponse<byte[]>();
        try
        {
            returnModel = await interviewService.ExportInterviewsToICSAsync(jobApplicationId);

            if (!returnModel.Success)
            {
                // Return an appropriate error response if the service fails
                return StatusCode((int)returnModel.StatusCode, new
                {
                    returnModel.Message,
                    returnModel.ErrorMessages
                });
            }

            // Set headers for downloading the .ics file
            Response.Headers.Add("Content-Disposition", "attachment; filename=interviews.ics");
            Response.ContentType = "text/calendar";

            // Return the .ics file as a byte array
            return File(returnModel.Data, "text/calendar");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Message = "An unexpected error occurred while exporting interviews.",
                Error = ex.Message
            });
        }
    }
}