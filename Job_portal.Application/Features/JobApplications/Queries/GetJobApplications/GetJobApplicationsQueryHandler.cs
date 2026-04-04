using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.JobApplications.Queries.GetJobApplications
{
    public class GetJobApplicationsQueryHandler : IRequestHandler<GetJobApplicationsQuery,GetJobApplicationsResult>
    {
        private readonly IJobApplicationRepository _applicationRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;

        public GetJobApplicationsQueryHandler(IJobApplicationRepository applicationRepository,IJobRepository jobRepository,IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _jobRepository = jobRepository;
            _mapper = mapper;
        }

        public async Task<GetJobApplicationsResult> Handle(GetJobApplicationsQuery request, CancellationToken ct)
        {
            var job = await _jobRepository.GetByIdForRecruiterAsync(request.JobId,request.RecruiterId,ct);
            if (job == null)
                return new GetJobApplicationsResult(false, "Job not found.");

            var applications = await _applicationRepository.GetByJobAsync(request.JobId, ct);

            // 3. Map applications
            var applicationDtos = _mapper
                .Map<List<ApplicationWithApplicantDto>>(applications);

            // 4. Build DTO manually — clear and explicit
            var jobDto = new JobWithApplicantsDto
            {   Id = job.Id,
                Title = job.Title,
                Applications = applicationDtos
            };

            return new GetJobApplicationsResult(true, "Applicants fetched.", jobDto);

        }
    }
}
