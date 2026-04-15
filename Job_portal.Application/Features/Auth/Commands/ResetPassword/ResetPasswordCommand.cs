using MediatR;

namespace Job_portal.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<ResetPasswordResult>
    {
        public string Token { get; init; } = string.Empty;
        public string NewPassword { get; init; } = string.Empty;
        public string ConfirmPassword { get; init; } = string.Empty;
    }

    public record ResetPasswordResult(bool Success, string Message);
}
