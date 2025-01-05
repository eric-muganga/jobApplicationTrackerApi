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
//[Authorize]
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
        if (!interviewsResponse.Success)
        {
            return BadRequest(interviewsResponse.Message);
        }

        var viewModel = mapper.Map<IEnumerable<InterviewView>>(interviewsResponse.Data);
        returnModel.Data = viewModel;
        returnModel.Success = interviewsResponse.Success;
        returnModel.Message = interviewsResponse.Message;

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

            returnModel = mapper.Map<ServiceResponse<InterviewView>>(interviewResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return CreatedAtAction(nameof(GetInterviewsByJobApplicationId), new { jobApplicationId = interviewView.JobApplicationId }, returnModel);
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

        return Ok(returnModel);
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
            returnModel = mapper.Map<ServiceResponse<bool>>(interviewResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(returnModel);
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
            var exportResponse = await interviewService.ExportInterviewsToICSAsync(jobApplicationId);
            if (!exportResponse.Success)
            {
                return BadRequest(exportResponse.Message);
            }

            byte[] returnFile = exportResponse.Data;
            returnModel.Message = exportResponse.Message;
            returnModel.StatusCode = exportResponse.StatusCode;

            return File(returnFile, "application/ics", "interviews.ics");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}