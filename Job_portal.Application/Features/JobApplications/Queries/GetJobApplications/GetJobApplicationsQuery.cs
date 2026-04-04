using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.JobApplications.Queries.GetJobApplications
{
    // GET /api/v1/application/{id}/applicants
    // recruiter views all applicants for their job
    // Applicants.jsx dispatches res.data.job to Redux
    public record GetJobApplicationsQuery : IRequest<GetJobApplicationsResult>
    {
        public Guid JobId { get; init; } // from URL
        public Guid RecruiterId { get; init; } // from JWT token
    }

    public record GetJobApplicationsResult(bool Success, string Message, JobWithApplicantsDto? Job = null);
}
