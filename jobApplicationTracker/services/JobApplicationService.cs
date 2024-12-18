using System.Net;
using Azure;
using jobApplicationTrackerApi.Data;
using jobApplicationTrackerApi.DataModels;
using Microsoft.EntityFrameworkCore;

namespace jobApplicationTrackerApi.services;

/// <summary>
/// Implements business logic for managing JobApplications. 
/// This class interacts with the database through EF Core and 
/// returns structured ServiceResponse objects to the caller.
/// </summary>
public class JobApplicationService : IJobApplicationService
{
    private readonly JobAppTrackerDbContext _context;

    // using dependency injection to pass the DbContext
    public JobApplicationService(JobAppTrackerDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Retrieves all job applications and returns them in a successful or error response.
    /// </summary>
    public async Task<ServiceResponse<IEnumerable<JobApplication>>> GetJobApplicationsAsync()
    {
        var response = new ServiceResponse<IEnumerable<JobApplication>>();

        try
        {
            var applications = await _context.JobApplications.ToListAsync();

            response.Data = applications;
            response.Message = "Job applications retrieved successfully";
            response.StatusCode = HttpStatusCode.OK;

        }
        catch (Exception ex) {
            response.Success = false;
            response.Data = null;
            response.Message = "Error";
            response.ErrorMessages = new List<string> { ex.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;

        }

        return response;

    }

    
    /// <summary>
    /// Retrieves a single job application by GUID. 
    /// If not found, returns a response indicating failure.
    /// </summary>
    public async Task<ServiceResponse<JobApplication>> GetJobApplicationByGuidAsync(Guid jobId)
    {
        var response = new ServiceResponse<JobApplication>();
        
        try
        {
            var application = await _context.JobApplications.FindAsync(jobId);

            if (application == null)
            {
                response.Success = false;
                response.Message = "Job application not found.";
                response.ErrorMessages = new List<string> { $"No application found with ID {jobId}" };
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                response.Data = application;
                response.Message = "Job application retrieved successfully.";
                response.StatusCode = HttpStatusCode.OK;
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Data = null;
            response.Message = "An error occurred while retrieving the job application.";
            response.ErrorMessages = new List<string> { ex.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }

        return response;
    }

    /// <summary>
    /// Add a new job application and saves it in the database.
    /// Returns the newly created entity.
    /// </summary>
    public async Task<ServiceResponse<JobApplication>> AddJobApplicationAsync(JobApplication jobApplication)
    {
        var response = new ServiceResponse<JobApplication>();
        try
        {
            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();
            
            response.Data = jobApplication;
            response.Message = "Job application added successfully.";
            response.StatusCode = HttpStatusCode.Created;
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Data = null;
            response.Message = "Error";
            response.ErrorMessages = new List<string> { e.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }
        
        return response;
    }

    /// <summary>
    /// Updates an existing job application.
    /// If the application does not exist, returns an error.
    /// </summary>
    public async Task<ServiceResponse<JobApplication>> UpdateJobApplicationAsync(JobApplication jobApplication)
    {
        var response = new ServiceResponse<JobApplication>();

        try
        {
            var existing = await _context.JobApplications.FindAsync(jobApplication.Id);
        
            if (existing == null)
            {
                response.Success = false;
                response.Message = "Job application not found.";
                response.ErrorMessages = new List<string> { $"No application found with ID {jobApplication.Id}" };
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
        
            // Update properties as needed.
            _context.Entry(existing).CurrentValues.SetValues(jobApplication);
            await _context.SaveChangesAsync();  
        
            response.Data = existing;
            response.Message = "Job application updated successfully.";
            response.StatusCode = HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Data = null;
            response.Message = "Error";
            response.ErrorMessages = new List<string> { e.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }
       
        return response;
    }

    /// <summary>
    /// Deletes a job application by ID. If not found,
    /// returns an error. If successful, returns true.
    /// </summary>
    public async Task<ServiceResponse<JobApplication>> DeleteJobApplicationAsync(JobApplication jobApplication)
    {
        var response = new ServiceResponse<JobApplication>();
        try
        {
            var existing =await _context.JobApplications.FindAsync(jobApplication.Id);

            if (existing == null)
            {
                response.Success = false;
                response.Message = "Job application not found.";
                response.ErrorMessages = new List<string> { $"No application found with ID {jobApplication.Id}" };
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
        
            _context.JobApplications.Remove(existing);
            await _context.SaveChangesAsync();
        
            response.Data = existing;
            response.Message = "Job application deleted successfully.";
            response.StatusCode = HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Data = null;
            response.Message = "Error";
            response.ErrorMessages = new List<string> { e.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }
        
        return response;
    }
}