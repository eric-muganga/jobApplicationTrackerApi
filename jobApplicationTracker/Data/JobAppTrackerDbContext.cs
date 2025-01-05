using jobApplicationTrackerApi.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace jobApplicationTrackerApi.Data;

public class JobAppTrackerDbContext : IdentityDbContext<ApplicationUser>
{
    public JobAppTrackerDbContext(DbContextOptions<JobAppTrackerDbContext> options): base(options){}
    
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<JobApplicationHistory> JobApplicationHistories { get; set; }
    public DbSet<ContractType> ContractTypes { get; set; }
    public DbSet<FinancialInformation> FinancialInformations { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    public DbSet<Status> Statuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // JobApplication entity
        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                 .HasDefaultValueSql("NEWID()");
            
            entity.Property(e => e.Company)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.JobTitle)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.HasOne(e => e.Status)
                .WithMany(s => s.JobApplications)
                .HasForeignKey(e => e.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.Property(e => e.ApplicationDate)
                .IsRequired();
            
            entity.HasOne(e => e.ContractType)
                .WithMany(ct => ct.JobApplications)
                .HasForeignKey(e => e.ContractTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(e => e.FinancialInformation)
                .WithMany(fi => fi.JobApplications)
                .HasForeignKey(e => e.FinancialInformationId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        // Status entity
        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.SortOrder)
                .IsRequired();

        });
        
        
        // interview entity
        modelBuilder.Entity<Interview>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(e => e.JobApplication)
                .WithMany(ja => ja.Interviews)
                .HasForeignKey(e => e.JobApplicationId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        
        // FinancialInformation entity
        modelBuilder.Entity<FinancialInformation>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Salary)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(10);
           
            entity.Property(e => e.SalaryType)
                .IsRequired()
                .HasMaxLength(30);
            
            entity.Property(e => e.TypeOfEmployment)
                .IsRequired()
                .HasMaxLength(50);
        });
        
        // ContractType entity
        modelBuilder.Entity<ContractType>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });
        
        
        // JobApplicationHistory entity
        modelBuilder.Entity<JobApplicationHistory>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            entity.HasOne(e => e.JobApplication)
                .WithMany()
                .HasForeignKey(e => e.JobApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Status)
                .WithMany()
                .HasForeignKey(e => e.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        
        // ApplicationUser configured by identity
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.FullName).HasMaxLength(150);
        });

    }
}