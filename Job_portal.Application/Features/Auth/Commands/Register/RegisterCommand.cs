using MediatR;

namespace Job_portal.Application.Features.Auth.Commands.Register
{
    public record RegisterCommand : IRequest<RegisterResult>
    {
        public string FullName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public Stream? PhotoStream { get; init; } 
        public string? PhotoFileName { get; init; }

    }

    public record RegisterResult
    (
        bool Success,
        string Message
    );
}
