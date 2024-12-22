
using System.Net;
using jobApplicationTrackerApi.Data;
using jobApplicationTrackerApi.DataModels;
using Microsoft.EntityFrameworkCore;

namespace jobApplicationTrackerApi.Services;

public class JobApplicationHistoryService :IJobApplicationHistoryService
{
    private readonly JobAppTrackerDbContext _context;
    
    public JobApplicationHistoryService(JobAppTrackerDbContext context)
    {
        _context = context;
    }
    public async Task<ServiceResponse<IEnumerable<JobApplicationHistory>>> GetHistoryByJobApplicationIdAsync(Guid jobApplicationId)
    {
        var response = new ServiceResponse<IEnumerable<JobApplicationHistory>>();
        try
        {
            var history = await _context.JobApplicationHistories
                .Include(h => h.Status)
                .Where(h => h.JobApplicationId == jobApplicationId)
                .OrderByDescending(h => h.UpdatedAt)
                .ToListAsync();

            response.Data = history;
            response.Message = "Job application history retrieved successfully.";
            response.StatusCode = HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "An error occurred while retrieving job application history.";
            response.ErrorMessages = new List<string> { ex.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }
        return response;
    }

    public async Task<ServiceResponse<JobApplicationHistory>> AddHistoryRecordAsync(JobApplicationHistory history)
    {
        var response = new ServiceResponse<JobApplicationHistory>();
        try
        {
            // Validate that the job application and status exist
            var jobExists = await _context.JobApplications.AnyAsync(j => j.Id == history.JobApplicationId);
            var statusExists = await _context.Statuses.AnyAsync(s => s.Id == history.StatusId);

            if (!jobExists || !statusExists)
            {
                response.Success = false;
                response.Message = "Invalid job application or status ID.";
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            _context.JobApplicationHistories.Add(history);
            await _context.SaveChangesAsync();

            response.Data = history;
            response.Message = "History record added successfully.";
            response.StatusCode = HttpStatusCode.Created;
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "An error occurred while adding the history record.";
            response.ErrorMessages = new List<string> { ex.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }

        return response;
    }
}