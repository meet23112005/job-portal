using Job_portal.Application.Common.Interfaces.Repositories;
using MediatR;

namespace Job_portal.Application.Features.SavedJobs.Commands.UnsaveJob
{
    public class UnsaveJobCommandHandler : IRequestHandler<UnsaveJobCommand,UnsaveJobResult>
    {
        private readonly ISavedJobRepository _savedJobRepository;
        private readonly IJobRepository _jobRepository;

        public UnsaveJobCommandHandler(ISavedJobRepository savedJobRepository,IJobRepository jobRepository )
        {
            _savedJobRepository = savedJobRepository;
            _jobRepository = jobRepository;
        }

        public async Task<UnsaveJobResult> Handle(UnsaveJobCommand request, CancellationToken ct)
        {
            // 1. Find saved job record
            var savedJob = await _savedJobRepository.GetByUserAndJobAsync(
                request.UserId, request.JobId, ct);
            if (savedJob == null)
                return new UnsaveJobResult(false, "Saved job not found.");

            // 2. Remove
            _savedJobRepository.Remove(savedJob);
            await _savedJobRepository.SaveChangesAsync(ct);

            return new UnsaveJobResult(true, "Job removed from saved list.");
        }
    }
}
