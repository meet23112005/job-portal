using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Jobs.Commands.UpdateJob
{
    public record UpdateJobCommand : IRequest<UpdateJobResult>
    {
        public Guid JobId { get; init; }
        public Guid RecruiterId {  get; init; } // from JWT token
        public string? Title { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public string? Requirenments { get; init; } = string.Empty;
        public decimal? Salary { get; init; }
        public string? Location { get; init; } = string.Empty;
        public string? JobType { get; init; } = string.Empty;
        public string? ExperienceLevel { get; init; } = string.Empty;
        public int? Position { get; init; }
    }
    public record UpdateJobResult(bool Success,string Message,JobDto? Job = null);
}
