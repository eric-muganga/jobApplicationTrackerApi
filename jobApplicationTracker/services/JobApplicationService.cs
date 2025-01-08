using System.Net;
using Azure;
using jobApplicationTrackerApi.Data;
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace jobApplicationTrackerApi.Services;

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
    public async Task<ServiceResponse<IEnumerable<JobApplication>>> GetJobApplicationsAsync(string userId)
    {
        var response = new ServiceResponse<IEnumerable<JobApplication>>();

        try
        {
            var applications = await _context.JobApplications
                .Where(x => x.UserId == Guid.Parse(userId))
                .Include(j => j.Status)
                .Include(j => j.ContractType)
                .Include(j => j.FinancialInformation)
                .ToListAsync();
            

            response.Data = applications;
            response.Message = "Job applications retrieved successfully";
            response.StatusCode = HttpStatusCode.OK;

        }
        catch (Exception ex) {
            response.Success = false;
            response.Data = null;
            response.Message = "An error occurred while retrieving job applications.";
            response.ErrorMessages = new List<string> { ex.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;

        }

        return response;

    }

    
    /// <summary>
    /// Retrieves a single job application by GUID. 
    /// If not found, returns a response indicating failure.
    /// </summary>
    public async Task<ServiceResponse<JobApplication>> GetJobApplicationByGuidAsync(Guid jobId, string userId)
    {
        var response = new ServiceResponse<JobApplication>();
        
        try
        {
            var application = await _context.JobApplications
                .Where(x => x.UserId == Guid.Parse(userId))
                .Include(j => j.Status)
                .Include(j => j.ContractType)
                .Include(j => j.FinancialInformation)
                .FirstOrDefaultAsync(j => j.Id == jobId);

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
/// Returns the newly created view model.
/// </summary>
public async Task<ServiceResponse<JobApplication>> AddJobApplicationAsync(JobApplication jobApplication)
{
    var response = new ServiceResponse<JobApplication>();   
    try
    {
        // Check if FinancialInformation is new (Id is empty)
        if (jobApplication.FinancialInformation?.Id == Guid.Empty)
        {
            var newFinancialInfo = new FinancialInformation
            {
                Salary = jobApplication.FinancialInformation.Salary,
                Currency = jobApplication.FinancialInformation.Currency,
                SalaryType = jobApplication.FinancialInformation.SalaryType,
                TypeOfEmployment = jobApplication.FinancialInformation.TypeOfEmployment
            };

            // Add the new FinancialInformation entity
            _context.FinancialInformations.Add(newFinancialInfo);
            await _context.SaveChangesAsync();

            // Assign the newly created FinancialInformationId to the JobApplication
            jobApplication.FinancialInformationId = newFinancialInfo.Id;
        }

        // Add the JobApplication entity
        _context.JobApplications.Add(jobApplication);
        await _context.SaveChangesAsync();

        // Load the Status nav property so we can get the string Name
        await _context.Entry(jobApplication).Reference(j => j.Status).LoadAsync();
        
        // Populate success response
        response.Data = jobApplication;
        response.Message = "Job application added successfully.";
        response.StatusCode = HttpStatusCode.Created;
        response.Success = true;
    }
    catch (Exception e)
    {
        // Populate error response
        response.Success = false;
        response.Data = null;
        response.Message = "An error occurred while adding the job application.";
        response.ErrorMessages = new List<string> { e.Message };
        response.StatusCode = HttpStatusCode.InternalServerError;
    }
    
    return response;
}

    /// <summary>
    /// Updates an existing job application.
    /// If the application does not exist, returns an error.
    /// </summary>
    public async Task<ServiceResponse<JobApplication>> UpdateJobApplicationAsync(JobApplication jobApplication, string userId)
    {
        var response = new ServiceResponse<JobApplication>();

        try
        {
            var existing = await _context.JobApplications
                .Where(x => x.UserId == Guid.Parse(userId))
                .FirstOrDefaultAsync(j => j.Id == jobApplication.Id);

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
            
            // Load Status or financial info if needed
            await _context.Entry(existing).Reference(j => j.Status).LoadAsync();
            await _context.Entry(existing)
                .Reference(j => j.FinancialInformation).LoadAsync();
        
            response.Data = existing;
            response.Message = "Job application updated successfully.";
            response.StatusCode = HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Data = null;
            response.Message = "An error occurred while updating the job application.";
            response.ErrorMessages = new List<string> { e.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }
       
        return response;
    }

    /// <summary>
    /// Deletes a job application by ID. If not found,
    /// returns an error. If successful, returns true.
    /// </summary>
    public async Task<ServiceResponse<JobApplication>> DeleteJobApplicationByIdAsync(Guid jobId, string userId)
    {

        var response = new ServiceResponse<JobApplication>();
        try
        {
            var existing = await _context.JobApplications
            .Where(x => x.UserId == Guid.Parse(userId))
            .FirstOrDefaultAsync(j => j.Id == jobId);

            if (existing == null)
            {
                response.Success = false;
                response.Message = "Job application not found.";
                response.ErrorMessages = new List<string> { $"No application found with ID {jobId}" };
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
            response.Message = "An error occurred while deleting the job application.";
            response.ErrorMessages = new List<string> { e.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }
        
        return response;
    }

    /// <summary>
    /// Updates the status of a job application based on a given statusId.
    /// Could be part of a simple workflow.
    /// </summary>
    public async Task<ServiceResponse<JobApplication>> UpdateJobApplicationStatusAsync(Guid jobApplicationId,
        Guid statusId, string userId)
    {
        var response = new ServiceResponse<JobApplication>();
        try
        {
            var existing = await _context.JobApplications
                .Where(x => x.UserId == Guid.Parse(userId))
                .FirstOrDefaultAsync(j => j.Id == jobApplicationId);

            if (existing == null)
            {
                response.Success = false;
                response.Message = "Job application not found.";
                response.ErrorMessages = new List<string> { $"No application found with ID {jobApplicationId}" };
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
            
            var statusExists = await _context.Statuses.AnyAsync(s => s.Id == statusId);

            if (!statusExists)
            {
                response.Success = false;
                response.Message = "Invalid Status.";
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            
            existing.StatusId = statusId;
            await _context.SaveChangesAsync();
            
            // **Load** the nav property so we can map Status.Name
            await _context.Entry(existing).Reference(j => j.Status).LoadAsync();
            
            response.Data = existing;
            response.Message = "Job application status updated successfully.";
            response.StatusCode = HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Data = null;
            response.Message = "An error occurred while updating the status job application.";
            response.ErrorMessages = new List<string> { e.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }
        
        return response;
    }

    /// <summary>
    /// Associates an employment type (ContractType) with the job application
    /// </summary>
    public async Task<ServiceResponse<JobApplication>> UpdateJobApplicationContractTypeAsync(Guid jobApplicationId,
        Guid contractTypeId, string userId)
    {
        var response = new ServiceResponse<JobApplication>();
        try
        {
            var existing = await _context.JobApplications
                .Where(x => x.UserId == Guid.Parse(userId))
                .FirstOrDefaultAsync(j => j.Id == jobApplicationId);

            if (existing == null)
            {
                response.Success = false;
                response.Message = "Job application not found.";
                response.ErrorMessages = new List<string> { $"No application found with ID {jobApplicationId}" };
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
            
            var contractTypeExists = await _context.ContractTypes.AnyAsync(c => c.Id == contractTypeId);
            if (!contractTypeExists)
            {
                response.Success = false;
                response.Message = "Invalid Contract Type.";
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }
            
            existing.ContractTypeId = contractTypeId;
            await _context.SaveChangesAsync();
            
            response.Data = existing;
            response.Message = "Job application contract type updated successfully.";
            response.StatusCode = HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Data = null;
            response.Message = "An error occurred while updating the Contract type job application.";
            response.ErrorMessages = new List<string> { e.Message };
            response.StatusCode = HttpStatusCode.InternalServerError;
        }
        
        return response;
    }
}