using Job_portal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Job_portal.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Job> Jobs { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<JobApplication> Applications { get; set; } = null!;
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<SavedJob> SavedJobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
           .HasIndex(U => U.Email)
           .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(u => u.User)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserProfile>()
                .Property(p => p.Skills)
                .HasConversion(
                v => string.Join(",", v),
                v => v.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .ToList());

            modelBuilder.Entity<Company>()
                .HasOne(C => C.Creator)
                .WithMany(C => C.Companies)
                .HasForeignKey(C => C.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Job>()
            .Property(j => j.Salary)
            .HasPrecision(18, 2);  // 111111111111111111.99

            modelBuilder.Entity<Job>()
                .HasOne(J => J.Company)
                .WithMany(C => C.Jobs)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Cascade); //Delete Company So Automatically Delete Jobs

            modelBuilder.Entity<Job>()
                .HasOne(J => J.Creator)
                .WithMany(U => U.Jobs)
                .HasForeignKey(U => U.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict); //cannot delete recruiter who has active jobs

            modelBuilder.Entity<JobApplication>()
                .Property(JA => JA.Status)
                .HasConversion<string>();

            modelBuilder.Entity<JobApplication>()
                .HasIndex(a => new { a.JobId, a.ApplicantId }) //prevents same student applying to same job twice
                .IsUnique();

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(ja => ja.JobId)
                .OnDelete(DeleteBehavior.Cascade); //job deleted → all its applications deleted

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.Applicant)
                .WithMany(u => u.Applications)
                .HasForeignKey(a => a.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);// cannot delete student who has applications and application history must be preserved

            modelBuilder.Entity<SavedJob>()
                .HasIndex(s => new { s.UserId, s.JobId })//student cannot save same job twice
                .IsUnique();

            modelBuilder.Entity<SavedJob>()
                .HasOne(SA => SA.User)
                .WithMany(u => u.SavedJobs)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavedJob>()
                .HasOne(s => s.Job)
                .WithMany(j => j.SavedJobs)
                .HasForeignKey(s => s.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
