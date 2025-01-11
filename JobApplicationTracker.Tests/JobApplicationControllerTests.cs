using AutoFixture;
using AutoMapper;
using jobApplicationTrackerApi.Controllers;
using jobApplicationTrackerApi.Data;
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.Services;
using jobApplicationTrackerApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace JobApplicationTracker.Tests;

[TestClass]
public sealed class JobApplicationControllerTests
{
    private JobAppTrackerDbContext _context;
    private JobApplicationController _controller;
    private IJobApplicationService _service;
    private IMapper _mapper;    
    public static JobAppTrackerDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<JobAppTrackerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new JobAppTrackerDbContext(options);
    }

    [TestInitialize]
    public void Setup()
    {
        _context = GetInMemoryDbContext();

        SeedDatabase();

        _service = new JobApplicationService(_context);

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

        _mapper = config.CreateMapper();

        _controller = new JobApplicationController(_service, _mapper);
    }

    private void SeedDatabase()
    {
        var fixture = new Fixture();

        // Statuses
        var toApplyStatus = new Status { Id = Guid.Parse("1fa85f64-5717-4562-b3fc-2c963f66af22"), Name = "ToApply" };
        var offerStatus = new Status { Id = Guid.Parse("2fa85f64-5717-4562-b3fc-2c963f66af22"), Name = "Offer" };

        // FinancialInformations
        var financialInfo1 = new FinancialInformation { Id = Guid.Parse("1fa85f64-5717-4562-b3fc-2c963f66af11"), Salary = "50000",
            Currency = "USD", 
            SalaryType = "Monthly", 
            TypeOfEmployment = "Full-time" };
        var financialInfo2 = new FinancialInformation { Id = Guid.Parse("2fa85f64-5717-4562-b3fc-2c963f66af11"), Salary = "100",
            Currency = "EUR",
            SalaryType = "Hourly",
            TypeOfEmployment = "Full-time"
        };

        // ContractTypes
        var b2bContract = new ContractType { Id = Guid.Parse("1fa85f64-5717-4562-b3fc-2c963f66af00"), Name = "B2B" };
        var umowaPracyContract = new ContractType { Id = Guid.Parse("2fa85f64-5717-4562-b3fc-2c963f66af00"), Name = "UmowaPracy" };
        var umowaZleceniaContract = new ContractType { Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66af00"), Name = "UmowaZlecenia" };

        // JobApplications
        var jobApplications = new List<JobApplication>
    {
        new JobApplication  //application date
        {
            Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa0"),
            JobTitle = "Software Developer",
            Company = "ABC Corp",
            StatusId = toApplyStatus.Id,
            ApplicationDate = DateTime.Now,
            FinancialInformationId = financialInfo1.Id,
            ContractTypeId = b2bContract.Id,
            Status = toApplyStatus,
            FinancialInformation = financialInfo1,
            ContractType = b2bContract
        },
        new JobApplication
        {
            Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb1"),
            JobTitle = "Data Analyst",
            Company = "XYZ Ltd",
            StatusId = offerStatus.Id,
            ApplicationDate = DateTime.Now,
            FinancialInformationId = financialInfo2.Id,
            ContractTypeId = umowaPracyContract.Id,
            Status = offerStatus,
            FinancialInformation = financialInfo2,
            ContractType = umowaPracyContract
        },
        new JobApplication
        {
            Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb2"),
            JobTitle = "Backend Engineer",
            Company = "DEF Inc",
            StatusId = toApplyStatus.Id,
            ApplicationDate = DateTime.Now,
            FinancialInformationId = financialInfo1.Id,
            ContractTypeId = umowaZleceniaContract.Id,
            Status = toApplyStatus,
            FinancialInformation = financialInfo1,
            ContractType = umowaZleceniaContract
        },
        new JobApplication
        {
            Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb3"),
            JobTitle = "Frontend Developer",
            Company = "GHI LLC",
            StatusId = offerStatus.Id,
            ApplicationDate = DateTime.Now,
            FinancialInformationId = financialInfo2.Id,
            ContractTypeId = b2bContract.Id,
            Status = offerStatus,
            FinancialInformation = financialInfo2,
            ContractType = b2bContract
        },
        new JobApplication
        {
            Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb4"),
            JobTitle = "Fullstack Developer",
            Company = "JKL Startup",
            StatusId = toApplyStatus.Id,
            ApplicationDate = DateTime.Now,
            FinancialInformationId = financialInfo1.Id,
            ContractTypeId = umowaPracyContract.Id,
            Status = toApplyStatus,
            FinancialInformation = financialInfo1,
            ContractType = umowaPracyContract
        }
    };

        // Adding object to the db
        _context.Statuses.AddRange(toApplyStatus, offerStatus);
        _context.FinancialInformations.AddRange(financialInfo1, financialInfo2);
        _context.ContractTypes.AddRange(b2bContract, umowaPracyContract, umowaZleceniaContract);
        _context.JobApplications.AddRange(jobApplications);


        _context.SaveChanges();
    }



    [TestMethod]
    public async Task GetJobApplications_ReturnsOk()
    {
        // Arrange: Everything set in Setup()

        // Act
        var result = await _controller.GetAllJobApplications();
        var contentResult = result as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        Assert.AreEqual(contentResult.StatusCode, 200);
        Assert.IsNotNull(contentResult.Value);
    }


    [TestMethod]
    public async Task GetJobApplicationById_ReturnsOk()
    {
        // Arrange: Everything set in Setup()

        // Act
        var result = await _controller.GetJobApplicationById(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa0"));
        var contentResult = result as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        Assert.AreEqual(contentResult.StatusCode, 200);
        Assert.IsNotNull(contentResult.Value);
        var JobAppModel = contentResult.Value as ServiceResponse<JobApplicationView>;
        Assert.IsNotNull(JobAppModel);
        Assert.AreEqual(JobAppModel.StatusCode, HttpStatusCode.OK);
        Assert.AreEqual(JobAppModel.Success, true);
        Assert.AreEqual(JobAppModel.Message, "Job application retrieved successfully.");
    }


    [TestMethod]
    public async Task GetJobApplicationById_ReturnsNotFound()
    {
        // Arrange: Everything set in Setup()

        // Act
        var result = await _controller.GetJobApplicationById(Guid.Parse("3fa85f64-5717-1111-b3fc-2c963f66afa0"));
        var contentResult = result as ObjectResult;

        // Assert
        Assert.IsNotNull(contentResult);
        Assert.AreEqual(contentResult.StatusCode, 404);
    }


    [TestMethod]
    public async Task CreateJobApplication_ReturnsOk()
    {
        // Arrange
        var newJobApplication = new JobApplicationView
        {
            Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afb5"),
            JobTitle = "Test Developer",
            Company = "Test Corp",
            StatusId = Guid.Parse("1fa85f64-5717-4562-b3fc-2c963f66af22"),
            ApplicationDate = DateTime.Now,
            Notes = "Good opportunity to switch to another good-paying job",
            ContractTypeId = Guid.Parse("2fa85f64-5717-4562-b3fc-2c963f66af00")
        };

        // Act
        var result = await _controller.CreateJobApplication(newJobApplication);
        var contentResult = result as ObjectResult;

        // Assert
        Assert.IsNotNull(contentResult);
        Assert.AreEqual(200, contentResult.StatusCode);

        var response = contentResult.Value as ServiceResponse<JobApplicationView>;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.Success);
        Assert.AreEqual(response.Message, "Job application added successfully.");

        // Check if it was added to the database
        Assert.AreEqual(6, _context.JobApplications.Count());
    }


    [TestMethod]
    public async Task CreateJobApplication_ReturnsInternalServerError()
    {
        // Arrange
        var newJobApplication = new JobApplicationView
        {
            //Case when user didn't provide any information
            //Most of the validation logic will lay in frontend and user somehow managed to pass it it should return 500
        };

        // Act
        var result = await _controller.CreateJobApplication(newJobApplication);
        var contentResult = result as ObjectResult;

        // Assert
        Assert.IsNotNull(contentResult);
        Assert.AreEqual(500, contentResult.StatusCode);

        var response = contentResult.Value as ServiceResponse<JobApplicationView>;
        Assert.IsNull(response);
    }



    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }
}