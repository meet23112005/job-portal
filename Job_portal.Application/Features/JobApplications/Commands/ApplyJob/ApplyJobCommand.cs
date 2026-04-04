using MediatR;

namespace Job_portal.Application.Features.JobApplications.Commands.ApplyJob
{

    // GET /api/v1/application/apply/{jobId}
    // student applies for a job
    public record ApplyJobCommand : IRequest<ApplyJobResult>
    {
        public Guid JobId { get; init; } // from URL
        public Guid ApplicantId { get; init; } // from JWT token
        public string? CoverLetter { get; init; } // optional
    }
    public record ApplyJobResult(bool Success, string Message);
}
