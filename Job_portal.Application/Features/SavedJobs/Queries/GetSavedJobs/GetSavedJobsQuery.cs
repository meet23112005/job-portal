using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.SavedJobs.Queries.GetSavedJobs
{
    public record GetSavedJobsQuery : IRequest<GetSavedJobsResult>
    {
        public Guid UserId { get; init; } // from JWT token
    }
    public record GetSavedJobsResult(bool Success, string Message, IEnumerable<JobDto>? Jobs = null);
}
