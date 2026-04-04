using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.Common.Models;
using Job_portal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Job_portal.Infrastructure.Persistence.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _context;

        public JobRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Job>> GetAllAsync(string? keyword, CancellationToken ct = default)
        {
            var query = _context.Jobs
                                .Include(J => J.Company)
                                .Include(J => J.Applications)
                                .Where(J => !J.IsRemoved);

            if(!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(J => 
                        J.Title.ToLower().Contains(keyword.ToLower()) ||
                        J.Location.ToLower().Contains(keyword.ToLower()) ||
                        J.JobType.ToLower().Contains(keyword.ToLower()));
            }

            return await query
                .OrderByDescending(J => J.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<Job?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Jobs.FindAsync(new object[] { id } , ct); //used in ApplyJob (just check job exists)
        }

        //ownership check — recruiter can only edit their own jobs
        public async Task<Job?> GetByIdForRecruiterAsync(Guid id, Guid recruiterId, CancellationToken ct = default)
        {
            return await _context.Jobs
                .FirstOrDefaultAsync(J => J.Id == id && J.CreatedBy == recruiterId,ct);
        }

        public async Task<Job?> GetByIdWithCompanyAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Jobs
                .Include(J => J.Company) //shows company name/logo on job detail page
                .Include(J => J.Applications)// needed for ApplicationCount in DTO
                .FirstOrDefaultAsync(J => J.Id == id && !J.IsRemoved,ct); //job with company and applications detail 
        }

        public async Task<List<Job>> GetByRecruiterAsync(Guid recruiterId, CancellationToken ct = default)
        {
            return await _context.Jobs
                        .Include(J => J.Company)
                        .Include(J => J.Applications)
                        .Where(J => J.CreatedBy == recruiterId && !J.IsRemoved)
                        .OrderByDescending(J => J.CreatedAt)
                        .ToListAsync(ct);
        }

        public async Task<(List<Job> Jobs, int Total)> GetPaginatedAsync(JobFilter filter, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var query = _context.Jobs
                .Include(J => J.Company)
                .Include(J => J.Applications)
                .Where(J => !J.IsRemoved)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Keyword))
                query = query.Where(j =>
                        j.Title.ToLower().Contains(filter.Keyword.ToLower()));

            if(!string.IsNullOrWhiteSpace(filter.Location))
                query = query.Where(j =>
                    j.Location.ToLower().Contains(filter.Location.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.JobType))
                query = query.Where(j =>
                    j.JobType.ToLower() == filter.JobType.ToLower());

            if (!string.IsNullOrWhiteSpace(filter.ExperienceLevel)) 
                query = query.Where(j =>
                     j.ExperienceLevel.ToLower().Contains(filter.ExperienceLevel.ToLower()));

            if(filter.MaxSalary.HasValue)
                query = query.Where(j => j.Salary <= filter.MaxSalary.Value);

            if (filter.MinSalary.HasValue)
                query = query.Where(j => j.Salary >= filter.MinSalary.Value);

            if(filter.CompanyId.HasValue)
                query = query.Where(j => j.CompanyId == filter.CompanyId.Value);

            var total = await query.CountAsync(ct);

            var jobs = await query
                .OrderByDescending(j => j.CreatedAt)
                .Skip((pageNumber-1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (jobs, total);
        }

        public void Add(Job job)
        {
            _context.Jobs.Add(job);
        }

        public void Remove(Job job)
        {
            _context.Jobs.Remove(job);
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}
