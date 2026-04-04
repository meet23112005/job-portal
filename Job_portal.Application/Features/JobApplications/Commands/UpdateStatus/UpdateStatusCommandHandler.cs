using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Domain.Enums;
using MediatR;

namespace Job_portal.Application.Features.JobApplications.Commands.UpdateStatus
{
    public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand, UpdateStatusResult>
    {
        private readonly IJobApplicationRepository _applicationRepository;
        public UpdateStatusCommandHandler(IJobApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<UpdateStatusResult> Handle(UpdateStatusCommand request, CancellationToken ct)
        {
            //Fetch application WITH job — one DB call
            //needs Job.CreatedBy to verify ownership
            var application = await _applicationRepository.GetByIdWithJobAsync(request.ApplicationId, ct);
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
            return new UpdateStatusResult(true,
                $"Application {request.Status.ToLower()} successfully.");
        }
    }
}
