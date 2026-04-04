using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Jobs.Commands.CreateJob
{
    public class CreateJobCommand : IRequest<CreateJobResult>
    {
        public Guid RecruiterId { get; init; } //From Jwt Token.
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty ;
        public string Requirements { get; init; } = string.Empty;
        public decimal Salary {  get; init; }
        public string Location { get; init; } = string.Empty;
        public string JobType { get; init; } = string.Empty;
        public string ExperienceLevel { get; init; } = string.Empty;
        public int Position {  get; init; }
        public Guid CompanyId { get; init; }
    }
    public record CreateJobResult(bool Success, string Message, JobDto? Job = null);
}
