using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Domain.Entities;
using MediatR;

namespace Job_portal.Application.Features.SavedJobs.Commands.SaveJob
{
    public class SaveJobCommandHandler : IRequestHandler<SaveJobCommand,SaveJobResult>
    {
        private readonly ISavedJobRepository _savedJobRepository;
        private readonly IJobRepository _jobRepository;

        public SaveJobCommandHandler(ISavedJobRepository savedJobRepository,IJobRepository jobRepository)
        {
            _savedJobRepository = savedJobRepository;
            _jobRepository = jobRepository;
        }

        public async Task<SaveJobResult> Handle(SaveJobCommand request, CancellationToken ct)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId, ct);
            if (job == null || job.IsRemoved ==true) 
                return new SaveJobResult (false,"Job Not Found.");

            var isAlreadySaved = await _savedJobRepository.AlreadySavedAsync(request.UserId,request.JobId, ct);
            if (isAlreadySaved)
                return new SaveJobResult(false, "Job Is Already Saved.");

            var savedJob = new SavedJob
            {
                UserId = request.UserId,
                JobId = request.JobId
            };

            _savedJobRepository.Add(savedJob);
            await _savedJobRepository.SaveChangesAsync(ct);

            return new SaveJobResult(true,"Job Saved Succesfully.");

        }
    }
}
