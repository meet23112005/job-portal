using Job_portal.Application.Common.Models;
using Job_portal.Domain.Entities;

namespace Job_portal.Application.Common.Interfaces.Repositories
{
    public interface IJobRepository
    {
        // GET /api/v1/job/get/{id} — ApplyJob existence check
        Task<Job?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // GET /api/v1/job/get/{id} — student job detail page
        Task<Job?> GetByIdWithCompanyAsync(Guid id, CancellationToken ct = default);

        // PUT,DELETE /api/v1/job/{id} — recruiter ownership check
        Task<Job?> GetByIdForRecruiterAsync(Guid id, Guid recruiterId, CancellationToken ct = default);


        // GET /api/v1/job/get?keyword= — student job listing
        Task<List<Job>> GetAllAsync(string? keyword, CancellationToken ct = default);

        // GET /api/v1/job/getadminjobs — recruiter's own jobs
        Task<List<Job>> GetByRecruiterAsync(Guid recruiterId, CancellationToken ct = default);

        // GET /api/v1/job/get with pagination + all filters
        Task<(List<Job> Jobs, int Total)> GetPaginatedAsync(
            JobFilter filter, int pageNumber, int pageSize, CancellationToken ct = default);

        void Add(Job job);
        void Remove(Job job);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
