using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Admin.Queries.GetJobWithapplicants
{
    // GET /api/v1/application/{id}/applicants (admin view)
    // admin views all applicants for any job
    public record GetJobWithApplicantsQuery : IRequest<GetJobWithApplicantsResult>
    {
        public Guid JobId { get; init; } // from URL
    }

    public record GetJobWithApplicantsResult(bool Success, string Message, JobWithApplicantsDto? Job = null);
}
