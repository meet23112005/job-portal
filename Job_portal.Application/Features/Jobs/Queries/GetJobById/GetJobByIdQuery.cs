using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Jobs.Queries.GetJobById
{
    // GET /api/v1/job/get/{id}
    // student views job detail page
    public record GetJobByIdQuery : IRequest<GetByJobIdResult>
    {
        public Guid JobId { get; init; }
    }
    public record GetByJobIdResult(bool Success,string Message,JobDto? Job = null);
}
