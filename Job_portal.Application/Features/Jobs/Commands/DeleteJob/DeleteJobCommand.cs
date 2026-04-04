using MediatR;

namespace Job_portal.Application.Features.Jobs.Commands.DeleteJob
{
    public record DeleteJobCommand : IRequest<DeleteJobResult>
    {
        public Guid JobId { get; init; }
        public Guid RecruiterId { get; init; } // from JWT token
        public bool IsAdmin { get; init; } // admin = hard delete
    }

    public record DeleteJobResult(bool Success, string Message);
}
