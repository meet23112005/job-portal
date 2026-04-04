using MediatR;

namespace Job_portal.Application.Features.JobApplications.Commands.UpdateStatus
{
    // POST /api/v1/application/status/{id}/update
    // recruiter accepts or rejects applicant
    public record UpdateStatusCommand :IRequest<UpdateStatusResult>
    {
        public Guid ApplicationId { get; init; } // from URL
        public Guid RecruiterId { get; init; } // from JWT token
        public string Status { get; init; } = string.Empty;
    }
    public record UpdateStatusResult(bool Success, string Message);
}
