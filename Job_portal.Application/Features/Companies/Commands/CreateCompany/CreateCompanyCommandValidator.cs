using FluentValidation;

namespace Job_portal.Application.Features.Companies.Commands.CreateCompany
{
    public  class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Company name is required.")
                .MaximumLength(150).WithMessage("Company name cannot exceed 150 characters.");
        }
    }
}
