using MediatR;

namespace Job_portal.Application.Features.SavedJobs.Commands.UnsaveJob
{
    // DELETE /api/v1/job/unsave/{jobId}
    // student removes a saved job
    public record UnsaveJobCommand :IRequest<UnsaveJobResult>
    {
        public Guid UserId { get; init; } // from JWT token
        public Guid JobId { get; init; } // from URL
    }
    public record UnsaveJobResult(bool Success,string Message);
}
