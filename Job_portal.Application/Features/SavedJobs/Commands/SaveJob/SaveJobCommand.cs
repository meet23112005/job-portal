using MediatR;

namespace Job_portal.Application.Features.SavedJobs.Commands.SaveJob
{
    // POST /api/v1/job/save/{jobId}
    // student saves a job for later
    public record SaveJobCommand : IRequest<SaveJobResult>
    {
        public Guid UserId { get; init; } // from JWT token
        public Guid JobId { get; init; } // from URL
    }
    public record SaveJobResult(bool Success,string Message);
}
