using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Jobs.Commands.UpdateJob
{
    public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand,UpdateJobResult>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;

        public UpdateJobCommandHandler(IJobRepository jobRepository,IMapper mapper)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
        }

        public async Task<UpdateJobResult> Handle(UpdateJobCommand request, CancellationToken ct)
        {
            //Fetch job — ownership check
            var job = await _jobRepository.GetByIdForRecruiterAsync(request.JobId,request.RecruiterId, ct);
            if (job == null)
                return new UpdateJobResult(false, "Job Not Found");

            if(!string.IsNullOrWhiteSpace(request.Title))
                job.Title = request.Title;

            if (!string.IsNullOrWhiteSpace(request.Description))
                job.Description = request.Description;

            if (!string.IsNullOrWhiteSpace(request.Location))
                job.Location = request.Location;

            if (!string.IsNullOrWhiteSpace(request.Requirenments))
                job.Requirements = request.Requirenments;

            if (request.Salary.HasValue && request.Salary > 0)
                job.Salary = request.Salary.Value;

            if (!string.IsNullOrWhiteSpace(request.JobType))
                job.JobType = request.JobType;

            if (!string.IsNullOrWhiteSpace(request.ExperienceLevel))
                job.ExperienceLevel = request.ExperienceLevel;

            if(request.Position.HasValue && request.Position > 0)
                job.Position = request.Position.Value;

            //Update in database
            await _jobRepository.SaveChangesAsync(ct);

            var jobDto = _mapper.Map<JobDto>(job);
            return new UpdateJobResult(true,"Job Succesfully Updated.", jobDto);
        }
    }
}
