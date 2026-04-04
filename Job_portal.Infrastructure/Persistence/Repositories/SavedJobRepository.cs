using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Job_portal.Infrastructure.Persistence.Repositories
{
    public class SavedJobRepository:ISavedJobRepository
    {
        private readonly AppDbContext _context;

        public SavedJobRepository(AppDbContext context)
        {
            _context = context;
        }
        

        public async Task<SavedJob?> GetByUserAndJobAsync(Guid userId, Guid jobId, CancellationToken ct = default)
        {
            return await _context.SavedJobs
                .FirstOrDefaultAsync(Sj => Sj.UserId == userId && Sj.JobId == jobId, ct);
        }

        public async Task<IEnumerable<SavedJob>> GetByUserAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.SavedJobs
                .Include(Sj => Sj.Job)
                    .ThenInclude(j => j!.Company)
                .Where(Sj => Sj.UserId == userId)
                .OrderByDescending(Sj => Sj.CreatedAt)
                .ToListAsync(ct);
        }
        public async Task<bool> AlreadySavedAsync(Guid userId, Guid jobId, CancellationToken ct = default)
        {
            return await _context.SavedJobs
                .AnyAsync(Sj => Sj.UserId == userId && Sj.JobId == jobId,ct);
        }

        public void Add(SavedJob savedJob)
        {
            _context.SavedJobs.Add(savedJob);
        }
        public void Remove(SavedJob savedJob)
        {
            _context.SavedJobs.Remove(savedJob);
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}
