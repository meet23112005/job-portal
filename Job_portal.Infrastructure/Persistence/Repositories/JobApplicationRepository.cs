using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Job_portal.Infrastructure.Persistence.Repositories
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly AppDbContext _context;

        public JobApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(JobApplication application)
        {
            _context.Applications.Add(application);
        }

        public async Task<bool> AlreadyAppliedAsync(Guid jobId, Guid applicantId, CancellationToken ct = default)
        {
            return await _context.Applications
                .AnyAsync(Ja => Ja.JobId == jobId
                                && Ja.ApplicantId == applicantId, ct);
        }

        //Student views their applied jobs list includes Job → shows title,salary,type includes Job.Company → shows company name,logo
        public async Task<IEnumerable<JobApplication>> GetByApplicantAsync(Guid applicantId, CancellationToken ct = default)
        {
            return await _context.Applications
                .Include(JA => JA.Job)
                    .ThenInclude(J => J!.Company)
                .Where(JA => JA.ApplicantId == applicantId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(JA => JA.Id == id, ct);
        }

        public async Task<JobApplication?> GetByIdWithJobAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Applications
                .Include(JA => JA.Job)
                .FirstOrDefaultAsync(JA => JA.Id == id, ct);
        }
        //Recruiter views applicants for their job
        //includes Applicant → shows name, email, phone & includes Applicant.Profile → shows resume
        public async Task<List<JobApplication>> GetByJobAsync(Guid jobId, CancellationToken ct = default)
        {
            return await _context.Applications
                .Include(JA => JA.Applicant)
                    .ThenInclude(u => u!.Profile)
                .Where(JA => JA.JobId == jobId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}
//No `Remove()` Method —  Why
//```
//Applications are NEVER deleted
//→ student sees their history
//→ recruiter sees past applicants
//→ only Status changes (Pending/Accepted/Rejected)
//→ no need for Remove()