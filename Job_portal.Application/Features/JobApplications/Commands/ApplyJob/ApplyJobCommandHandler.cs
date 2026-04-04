using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Domain.Entities;
using MediatR;

namespace Job_portal.Application.Features.JobApplications.Commands.ApplyJob
{
    public class ApplyJobCommandHandler:IRequestHandler<ApplyJobCommand,ApplyJobResult>
    {
        private readonly IJobApplicationRepository _jobApplicationRepo;
        private readonly IJobRepository _jobRepo;

        public ApplyJobCommandHandler(IJobApplicationRepository jobApplicationRepo , IJobRepository jobRepo)
        {
            _jobApplicationRepo = jobApplicationRepo;
            _jobRepo = jobRepo;
        }

        public async Task<ApplyJobResult> Handle(ApplyJobCommand request, CancellationToken ct)
        {
            // 1. Check job exists
            var job = await _jobRepo.GetByIdAsync(request.JobId, ct);
            if (job == null)
                return new ApplyJobResult(false, "Job not found.");

            //check Job Is Not Removed.
            if(job.IsRemoved)
                return new ApplyJobResult(false, "Job is no longer available.");

            //Check duplicate application
            var alreadyApplied = await _jobApplicationRepo.AlreadyAppliedAsync(request.JobId, request.ApplicantId, ct);
            if(alreadyApplied)
                return new ApplyJobResult(false, "You have already applied for this job.");

            //create Application 
            var application = new JobApplication
            {
                ApplicantId = request.ApplicantId,
                JobId = request.JobId,
                Status = Domain.Enums.ApplicationStatus.Pending,
                CoverLetter = request.CoverLetter
            };

            _jobApplicationRepo.Add(application);
            await _jobApplicationRepo.SaveChangesAsync(ct);

            return new ApplyJobResult(true, "Application Submitted Successfully.");
        }
    }
}
