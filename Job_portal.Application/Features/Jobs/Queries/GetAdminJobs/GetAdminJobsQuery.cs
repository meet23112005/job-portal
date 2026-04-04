using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Jobs.Queries.GetAdminJobs
{
    // GET /api/v1/job/getadminjobs
    // recruiter views their own posted jobs
    public record GetAdminJobsQuery : IRequest<GetAdminJobsResult>
    {
        public Guid RecruiterId { get; init; } // from JWT token
    }

    public record GetAdminJobsResult(
        bool Success,
        string Message,
        IEnumerable<JobDto>? Jobs = null);
}
