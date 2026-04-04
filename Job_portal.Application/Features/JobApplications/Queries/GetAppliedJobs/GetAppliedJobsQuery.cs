using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.JobApplications.Queries.GetAppliedJobs
{
    // GET /api/v1/application/get
    // student views their applied jobs list
    public record GetAppliedJobsQuery : IRequest<GetAppliedJobsResult>
    {
        public Guid ApplicantId { get; init; }  //From Jwt Token
    }

    public record GetAppliedJobsResult(bool Success, string Message,IEnumerable<JobApplicationDto>? Application = null);
}
