using FluentValidation;

namespace Job_portal.Application.Features.Auth.Commands.UpdateProfileCommand
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("please Enter a Valid Email.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"[6-9]\d{9}$").WithMessage("Phone number must be 10 digits.")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

            RuleFor(x => x.FullName)
                .MaximumLength(100).WithMessage("Full Name Can not exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.FullName));

            RuleFor(x => x.Bio)
                .MaximumLength(500).WithMessage("Message can not exceed 500 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.Bio));
                
        }
    }
}
