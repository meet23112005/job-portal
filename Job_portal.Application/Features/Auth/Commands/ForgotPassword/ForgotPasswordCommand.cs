using MediatR;

namespace Job_portal.Application.Features.Auth.Commands.ForgotPassword
{
    public record ForgotPasswordCommand : IRequest<ForgotPasswordResult>
    {
        public string Email { get; init ; } =string.Empty;
    }
    public record ForgotPasswordResult(bool Success,string Message);
}
