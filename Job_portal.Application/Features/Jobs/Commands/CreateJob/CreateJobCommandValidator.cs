using FluentValidation;

namespace Job_portal.Application.Features.Jobs.Commands.CreateJob
{
    public class CreateJobCommandValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Job Title is Required.")
                .MaximumLength(200).WithMessage("Title cannot Exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Job Discription is Required.")
                .MaximumLength(2000).WithMessage("Description cannot Exceed 2000 characters.");

            RuleFor(x => x.Requirements)
                .NotEmpty().WithMessage("Job Requirenments is Required.");

            RuleFor(x => x.Salary)
                .GreaterThan(0).WithMessage("Salary must be greater than zero.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Job Location is Required.");

            RuleFor(x => x.JobType)
                .NotEmpty().WithMessage("Job Type is Required.");

            RuleFor(x => x.ExperienceLevel)
                .NotEmpty().WithMessage("Job Experience Level is Required.");

            RuleFor(x => x.Position)
                .GreaterThan(0).WithMessage("Position must be greater than 0.");

            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company is required.");
        }
    }
}
