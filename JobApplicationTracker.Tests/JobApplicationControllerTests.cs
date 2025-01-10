using AutoFixture;
using jobApplicationTrackerApi.Controllers;
using jobApplicationTrackerApi.Data;
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;

namespace JobApplicationTracker.Tests;

[TestClass]
public sealed class JobApplicationControllerTests
{
    private JobAppTrackerDbContext _context;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<JobAppTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: "JobApplicationTestDb")
            .Options;

        _context = new JobAppTrackerDbContext(options);

        SeedDatabase();
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
        new JobApplication
        {
            Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa0"),
            JobTitle = "Software Developer",
            Company = "ABC Corp",
            StatusId = toApplyStatus.Id,
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


    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }

    [TestMethod]
    public void GetJobApplicationReturnsOk()
    {
        //Arrange
        //Act 
        //Assert
    }
}