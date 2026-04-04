using Job_portal.Application.Common.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.Features.Jobs.Commands.DeleteJob
{
    public class DeleteJobCommandHandler : IRequestHandler<DeleteJobCommand, DeleteJobResult>
    {
        private readonly IJobRepository _jobRepository;

        public DeleteJobCommandHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<DeleteJobResult> Handle(DeleteJobCommand request, CancellationToken ct)
        {
            var job = await _jobRepository.GetByIdForRecruiterAsync(request.JobId, request.RecruiterId, ct);
            if (job == null)
                return new DeleteJobResult(false, "Job Not Found.");

            if (request.IsAdmin)
            {
                // Admin → hard delete — removes from DB completely
                _jobRepository.Remove(job);
            }
            else
            {
                // Recruiter → soft delete — just hide from listings
                job.IsRemoved = true;
            }

            await _jobRepository.SaveChangesAsync(ct);
            return new DeleteJobResult(true, "Job Succesfully Deleted.");
        }
    }
}
