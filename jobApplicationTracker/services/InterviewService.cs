using System.Net;
using jobApplicationTrackerApi.Data;
using jobApplicationTrackerApi.DataModels;
using Microsoft.EntityFrameworkCore;

namespace jobApplicationTrackerApi.Services;

public class InterviewService : IInterviewService
{
    private readonly JobAppTrackerDbContext _context;

    public InterviewService(JobAppTrackerDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<ServiceResponse<IEnumerable<Interview>>> GetInterviewsByJobApplicationIdAsync(Guid jobApplicationId)
    {
        var response = new ServiceResponse<IEnumerable<Interview>>();
        try
        {
            var interviews = await _context.Interviews
                .Where(i => i.JobApplicationId == jobApplicationId)
                .ToListAsync();

            response.Data = interviews;
            response.Message = "Interviews retrieved successfully.";
            return response;
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = "Error occurred while retrieving interviews.";
            response.ErrorMessages = new List<string> { e.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }
        
        return response;
    }

    public async Task<ServiceResponse<Interview>> AddInterviewAsync(Interview interview)
    {
        var response = new ServiceResponse<Interview>();
        try
        {
            // Validate that the associated JobApplicationId exists
            var jobApplicationExists = await _context.JobApplications
                .AnyAsync(j => j.Id == interview.JobApplicationId);
            if (!jobApplicationExists)
            {
                response.Success = false;
                response.Message = "Invalid Job Application ID.";
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            _context.Interviews.Add(interview);
            await _context.SaveChangesAsync();

            response.Data = interview;
            response.Message = "Interview added successfully.";
            response.StatusCode = HttpStatusCode.Created;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "An error occurred while adding the interview.";
            response.ErrorMessages = new List<string> { ex.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }

        return response;
    }
    

    public async Task<ServiceResponse<Interview>> UpdateInterviewAsync(Interview interview)
    {
        var response = new ServiceResponse<Interview>();
        try
        {
            var existing = await _context.Interviews.FindAsync(interview.Id);
            if (existing == null)
            {
                response.Success = false;
                response.Message = "Interview not found.";
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            _context.Entry(existing).CurrentValues.SetValues(interview);
            await _context.SaveChangesAsync();

            response.Data = existing;
            response.Message = "Interview updated successfully.";
            response.StatusCode = HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "An error occurred while updating the interview.";
            response.ErrorMessages = new List<string> { ex.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }

        return response;
    }

    
    public async Task<ServiceResponse<bool>> DeleteInterviewAsync(Guid interviewId)
    {
        var response = new ServiceResponse<bool>();
        try
        {
            var existing = await _context.Interviews.FindAsync(interviewId);
            if (existing == null)
            {
                response.Success = false;
                response.Message = "Interview not found.";
                response.StatusCode = HttpStatusCode.NotFound;
                response.Data = false;
                return response;
            }

            _context.Interviews.Remove(existing);
            await _context.SaveChangesAsync();

            response.Data = true;
            response.Message = "Interview deleted successfully.";
            response.StatusCode = HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Data = false;
            response.Message = "An error occurred while deleting the interview.";
            response.ErrorMessages = new List<string> { ex.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }

        return response;
    }

    
    public async Task<ServiceResponse<byte[]>> ExportInterviewsToICSAsync(Guid jobApplicationId)
    {
        var response = new ServiceResponse<byte[]>();
        try
        {
            // Retrieve interviews
            var interviews = await _context.Interviews
                .Where(i => i.JobApplicationId == jobApplicationId)
                .ToListAsync();

            // Convert interviews to ICS format (placeholder logic)
            byte[] icsData = GenerateICSFile(interviews);

            response.Data = icsData;
            response.Message = "ICS file generated successfully.";
            response.StatusCode = HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "An error occurred while generating the ICS file.";
            response.ErrorMessages = new List<string> { ex.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }

        return response;
    }
    
    
    /// <summary>
    /// Placeholder method for generating an ICS file from interview data.
    /// Replace this with actual ICS generation logic.
    /// </summary>
    private byte[] GenerateICSFile(IEnumerable<Interview> interviews)
    {
        // Implement ICS generation logic here
        return new byte[0];
    }
}