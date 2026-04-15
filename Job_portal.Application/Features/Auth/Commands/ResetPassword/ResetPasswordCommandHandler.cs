using Job_portal.Application.Common.Interfaces.Repositories;
using MediatR;

namespace Job_portal.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResult>
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResetPasswordResult> Handle(ResetPasswordCommand request, CancellationToken ct)
        {
            if(request.NewPassword != request.ConfirmPassword)
            {
                return new ResetPasswordResult(false, "Passwords do not match.");
            }

            var user = await _userRepository.GetByResetTokenAsync(request.Token, ct);
            if (user == null)
                return new ResetPasswordResult(false,"Reset link is invalid or has expired.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            user.PasswordResetToken = null;
            user.ResetTokenExpiresAt = null;
            await _userRepository.SaveChangesAsync(ct);

            return new ResetPasswordResult(true, "Password has been reset successfully.");

        }
    }
}
