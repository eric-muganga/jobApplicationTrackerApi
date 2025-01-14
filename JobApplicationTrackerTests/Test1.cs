namespace JobApplicationTrackerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.EntityFrameworkCore;
//using jobApplicationTrackerApi.Controllers;
//using jobApplicationTrackerApi.Data;
//using jobApplicationTrackerApi.DataModels;
//using jobApplicationTrackerApi.Services;
//using jobApplicationTrackerApi.ViewModels;
//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public class JobAppTrackerDbContext : DbContext
{
    public DbSet<JobApplication> JobApplications { get; set; }

    public JobAppTrackerDbContext(DbContextOptions<JobAppTrackerDbContext> options) : base(options) { }
}

[TestClass]
public class JobApplicationControllerTests
{
    private JobAppTrackerDbContext _context;
    private JobApplicationController _controller;

    [TestInitialize]
    public void Setup()
    {
        // Configure in-memory database
        var options = new DbContextOptionsBuilder<JobAppTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new JobAppTrackerDbContext(options);

        // Seed the database with test data
        SeedDatabase();

        // Instantiate the controller
        var service = new JobApplicationService(_context); // Assuming this is your service
        _controller = new JobApplicationController(service);
    }

    private void SeedDatabase()
    {
        _context.JobApplications.AddRange(
            new JobApplication { Id = Guid.NewGuid(), Title = "Software Developer", Company = "ABC Corp" },
            new JobApplication { Id = Guid.NewGuid(), Title = "Data Analyst", Company = "XYZ Ltd" }
        );

        _context.SaveChanges();
    }

    [TestMethod]
    public async Task GetAllJobApplications_ShouldReturnAllApplications()
    {
        // Act
        var result = await _controller.GetAllJobApplications();
        var okResult = result as OkObjectResult;

        // Assert
        Assert.IsNotNull(okResult);
        var jobApplications = okResult.Value as List<JobApplicationViewModel>;
        Assert.AreEqual(2, jobApplications.Count);
    }

    [TestMethod]
    public async Task CreateJobApplication_ShouldAddApplication()
    {
        // Arrange
        var newJobApplication = new JobApplicationViewModel
        {
            Title = "QA Engineer",
            Company = "Tech Solutions"
        };

        // Act
        var result = await _controller.CreateJobApplication(newJobApplication);
        var okResult = result as OkObjectResult;

        // Assert
        Assert.IsNotNull(okResult);
        Assert.AreEqual("Job application created successfully.", okResult.Value);

        // Verify database update
        var applications = _context.JobApplications.ToList();
        Assert.AreEqual(3, applications.Count);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }

    public void GetJobApplicationsReturnsOk()
    {
        //Arrange

        //Act 

        //Assert
    }

// Read(different variants) Create(correct/incorrect) Update(correct/incorrect) Delete(delete, then read if it was deleted)
}
/*public class TaskControllerTests
{
    public static Guid guid = new Guid("acfed629-db10-48e6-866d-413746ce39ab");
    [TestMethod]
    public void GetTaskReturnsOk()
    {
        // Arrange
        var mockRepository = new Mock<ITaskService>();
        mockRepository.Setup(x => x.GetTaskById(guid, "00000000-0000-0000-0000-000000000000"))
            .Returns(        
             new TaskModel
            {
                Id = guid,
                Name = "MyName",
                Description = "MyDescription",
                ActionDate = DateTime.Now,
                GroupId = guid
            });

        var controller = new TaskController(mockRepository.Object);

        // Act
        IActionResult actionResult = controller.GetTask(guid);
        var contentResult = actionResult as OkObjectResult;

        // Assert
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(contentResult.StatusCode, 200);
        Assert.IsNotNull(contentResult.Value);
        var taskModel = contentResult.Value as TaskModel;
        Assert.IsNotNull(taskModel);
        Assert.AreEqual(taskModel.Name, "MyName");
        Assert.AreEqual(taskModel.Id, guid);
        Assert.AreEqual(taskModel.Description, "MyDescription");

    }


    [TestMethod]
    public void GetTaskReturnsNotFound()
    {
        // Arrange
        var mockRepository = new Mock<ITaskService>();
        mockRepository.Setup(x => x.GetTaskById(guid, "00000000-0000-0000-0000-000000000000"))
            .Returns<TaskModel>(null);

        var controller = new TaskController(mockRepository.Object);

        // Act
        IActionResult actionResult = controller.GetTask(guid);

        //// Assert
        Assert.IsNotNull(actionResult);
        Assert.AreEqual(((StatusCodeResult)actionResult).StatusCode, 404);
    }
}*/
