using Job_portal.Application.Common.Interfaces;
using Job_portal.Application.Common.Interfaces.Repositories;
using MediatR;

namespace Job_portal.Application.Features.Auth.Commands.ForgotPassword.Commands
{
    public class ForgotPasswordCommandHandler :IRequestHandler<ForgotPasswordCommand, ForgotPasswordResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(IUserRepository userRepository,IEmailService emailService) 
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<ForgotPasswordResult> Handle(ForgotPasswordCommand request, CancellationToken ct)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email,ct);
            if(user == null)
            {
                return new ForgotPasswordResult
                (
                    false,  "User with the provided email does not exist."
                );
            }

            // Generate a password reset token and send it via email
            var resetToken = Guid.NewGuid().ToString();
            user.PasswordResetToken = resetToken;
            user.ResetTokenExpiresAt = DateTime.UtcNow.AddHours(1);
            await _userRepository.SaveChangesAsync(ct);

            var resetLink = $"http://localhost:5173/reset-password?token={resetToken}";

            await _emailService.SendForgotPasswordEmailAsync(user.Email, user.FullName,resetLink);

            return new ForgotPasswordResult(
                true,
                "If this email is registered, a reset link has been sent.");


        }
    }
}
