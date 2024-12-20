using jobApplicationTrackerApi.Data;
using jobApplicationTrackerApi.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<JobAppTrackerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JobTrackerDb")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<JobAppTrackerDbContext>();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
