using Job_portal.Application.Common.Interfaces;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Domain.Enums;
using MediatR;

namespace Job_portal.Application.Features.JobApplications.Commands.UpdateStatus
{
    public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand, UpdateStatusResult>
    {
        private readonly IJobApplicationRepository _applicationRepository;
        private readonly IEmailService _emailService;

        public UpdateStatusCommandHandler(IJobApplicationRepository applicationRepository,IEmailService emailService)
        {
            _applicationRepository = applicationRepository;
            _emailService = emailService;
        }

        public async Task<UpdateStatusResult> Handle(UpdateStatusCommand request, CancellationToken ct)
        {
            //Fetch application WITH job — one DB call
            //needs Job.CreatedBy to verify ownership
            var application = await _applicationRepository.GetByIdWithApplicantAndJobAsync(request.ApplicationId, ct);
            if (application == null)
                return new UpdateStatusResult(false, "Application not found.");

            // Verify recruiter owns the job
            if (application.Job?.CreatedBy != request.RecruiterId)
                return new UpdateStatusResult(false, "Not Authorized.");

            application.Status = request.Status.ToLower() switch
            {
                "accepted" => ApplicationStatus.Accepted,
                "rejected" => ApplicationStatus.Rejected,
                _ => ApplicationStatus.Pending
            };

            await _applicationRepository.SaveChangesAsync(ct);

            if(application.Status != ApplicationStatus.Pending)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _emailService.SendApplicationStatusEmailAsync(
                            toEmail: application.Applicant!.Email,
                            fullName: application.Applicant!.FullName,
                            jobTitle: application.Job!.Title,
                            companyName: application.Job!.Company?.Name ?? "the Company",
                            status: request.Status.ToLower());
                    }
                    catch (Exception ex)
                    {
                        // log but don't crash the request
                        Console.WriteLine($"Email failed: {ex.Message}");
                    }
                });
            }

            return new UpdateStatusResult(true,
                $"Application {request.Status.ToLower()} successfully.");
        }
    }
}
