using MediatR;

namespace Job_portal.Application.Features.Auth.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand : IRequest<ConfirmEmailResult>
    {
        public string Token { get; init; } = string.Empty;
    }

    public record ConfirmEmailResult
    (
        bool Success,
        string Message
    );  
}
