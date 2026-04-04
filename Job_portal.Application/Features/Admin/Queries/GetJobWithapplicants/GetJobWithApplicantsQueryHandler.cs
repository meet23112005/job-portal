using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Admin.Queries.GetJobWithapplicants
{
    public class GetJobWithApplicantsQueryHandler : IRequestHandler<GetJobWithApplicantsQuery,GetJobWithApplicantsResult>
    {
        private readonly IJobRepository _jobRepo;
        private readonly IJobApplicationRepository _applicationRepo;
        private readonly IMapper _mapper;

        public GetJobWithApplicantsQueryHandler(
            IJobRepository jobRepo,
            IJobApplicationRepository applicationRepo,
            IMapper mapper)
        {
            _jobRepo = jobRepo;
            _applicationRepo = applicationRepo;
            _mapper = mapper;
        }

        public async Task<GetJobWithApplicantsResult> Handle(GetJobWithApplicantsQuery request, CancellationToken ct)
        {
            //  Get job — NO ownership check (admin sees all)
            var job = await _jobRepo.GetByIdAsync(request.JobId, ct);
            if (job == null)
                return new GetJobWithApplicantsResult(false, "Job Not Found.");

            var applications = await _applicationRepo.GetByJobAsync(request.JobId, ct);

            //Build DTO manually
            var applicationDtos = _mapper
                .Map<List<ApplicationWithApplicantDto>>(applications);

            var jobDto = new JobWithApplicantsDto
            {
                Id = job.Id,
                Title = job.Title,
                Applications = applicationDtos
            };
            return new GetJobWithApplicantsResult(true, "Job with applicants fetched.", jobDto);
        }
    }

}
