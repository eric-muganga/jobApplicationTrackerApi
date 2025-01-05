using System.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using jobApplicationTrackerApi.Data;
using jobApplicationTrackerApi.DataModels;
using Microsoft.EntityFrameworkCore;

using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            response.StatusCode = HttpStatusCode.OK;
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
            // Check if the JobApplication exists
            var jobApplicationExists = await _context.JobApplications
                .AnyAsync(j => j.Id == jobApplicationId);

            if (!jobApplicationExists)
            {
                response.Success = false;
                response.Message = $"JobApplication with ID {jobApplicationId} does not exist.";
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            // Check if jobApplication contains any interviews
            var interviews = await _context.Interviews
                .Where(i => i.JobApplicationId == jobApplicationId)
                .ToListAsync();
            if (interviews.Count == 0)
            {
                response.Success = false;
                response.Message = $"JobApplication with ID {jobApplicationId} does not contain any Interviews.";
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            // Retrieve interviews
            var interviewsResponse = await _context.Interviews
                .Where(i => i.JobApplicationId == jobApplicationId)
                .ToListAsync();

            // Convert interviews to ICS format (placeholder logic)
            byte[] icsData = GenerateICSFile(interviewsResponse);

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
    private byte[] GenerateICSFile(IEnumerable<Interview> interviewsResponse)
    {
        // Create a new calendar
        var calendar = new Calendar();

        // Convert each interview to an iCalendar event
        foreach (var interview in interviewsResponse)
        {
            if (interview.Date == null)
            {
                continue; // Skip if Date is not specified
            }

            var startTime = interview.Date.Value;
            var endTime = startTime.AddHours(1);

            var calendarEvent = new CalendarEvent
            {
                Summary = interview.Type ?? "Interview", // Type as the summary
                Description = interview.Notes, // Notes as the description
                //Location = interview.Location, // Use Location if available
                DtStart = new CalDateTime(startTime), // Start time
                DtEnd = new CalDateTime(endTime), // End time
                Uid = interview.Id.ToString() // Unique identifier
            };

            // Add Location only if it's not null or empty
            //if (!string.IsNullOrWhiteSpace(interview.Location))
            //{
            //    calendarEvent.Location = interview.Location;
            //}

            calendar.Events.Add(calendarEvent);
        }

        // Serialize the calendar to an iCalendar (.ics) format
        var serializer = new CalendarSerializer();
        var serializedCalendar = serializer.SerializeToString(calendar);

        // Convert the serialized string to a byte array
        var icsData = System.Text.Encoding.UTF8.GetBytes(serializedCalendar);

        return icsData;
    }
}