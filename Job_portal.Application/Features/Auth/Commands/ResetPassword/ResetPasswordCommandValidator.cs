using FluentValidation;

namespace Job_portal.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator
        : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Reset token is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Matches(@"(?=.*[A-Z])").WithMessage("Must contain uppercase letter.")
                .Matches(@"(?=.*[a-z])").WithMessage("Must contain lowercase letter.")
                .Matches(@"(?=.*\d)").WithMessage("Must contain a digit.")
                .Matches(@"(?=.*[\W_])").WithMessage("Must contain a special character.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match.");
        }
    }
}