using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Auth.Commands.UpdateProfileCommand
{
    public record UpdateProfileCommand : IRequest<UpdateProfileResult>
    {
        public Guid UserId { get; init; }
        public string? FullName { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Bio { get; init; }
        public List<string>? Skills {  get; init; }
        public Stream? PhotoStream {  get; init; }
        public string? PhotoFileName { get; init; }
        public Stream? ResumeStream { get; init; }
        public string? ResumeFileName {  get; init; }
        public string? ResumeOriginalName { get; init; }
    }
    public record UpdateProfileResult(bool Success, string Message, UserDto? User = null);
}
