using Job_portal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.Common.Interfaces.Repositories
{
    public interface ISavedJobRepository
    {
        // GET /api/v1/job/save/{jobId} — finds exact saved record for unsave feature
        Task<SavedJob?> GetByUserAndJobAsync(Guid userId,Guid jobId, CancellationToken ct = default);

        // GET /api/v1/job/saved — student views saved jobs with Job + Company data
        Task<IEnumerable<SavedJob>> GetByUserAsync(Guid userId, CancellationToken ct = default);


        Task<bool> AlreadySavedAsync(Guid userId, Guid jobId, CancellationToken ct = default);

        void Add(SavedJob savedJob);
        void Remove(SavedJob savedJob);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
