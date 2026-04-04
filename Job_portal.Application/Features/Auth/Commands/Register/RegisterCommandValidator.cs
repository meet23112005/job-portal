using FluentValidation;

namespace Job_portal.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name must be required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("email must be required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is reuired.")
                .Matches(@"^[6-9]\d{9}$").WithMessage("Phone number must be 10 digits and first digit should be between 6-9.");
                
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("PAssword must be required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Matches(@"(?=.*[A-Z])").WithMessage("Password must contain at least one upper letter.")
                .Matches(@"(?=.*[a-z])").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"(?=.*\d)").WithMessage("password must contain at least one digit")
                .Matches(@"(?=.*[\W_])").WithMessage("password must containat least one special character");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(r => r.ToLower() == "student" ||  r.ToLower() == "recruiter")
                .WithMessage("Role must be student or recruiter.");
        }
    }
}
