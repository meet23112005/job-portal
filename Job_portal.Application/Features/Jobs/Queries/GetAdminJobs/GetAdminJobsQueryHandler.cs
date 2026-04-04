using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Jobs.Queries.GetAdminJobs
{
    public record GetAdminJobsQueryHandler : IRequestHandler<GetAdminJobsQuery, GetAdminJobsResult>
    {
        private readonly IJobRepository _jobRepo;
        private readonly IMapper _mapper;

        public GetAdminJobsQueryHandler(IJobRepository jobRepo, IMapper mapper)
        {
            _jobRepo = jobRepo;
            _mapper = mapper;
        }

        public async Task<GetAdminJobsResult> Handle(GetAdminJobsQuery request, CancellationToken ct)
        {
            // recruiter's own jobs only
            var jobs = await _jobRepo.GetByRecruiterAsync(request.RecruiterId, ct);

            var jobDtos = _mapper.Map<IEnumerable<JobDto>>(jobs);
            return new GetAdminJobsResult(true, "Jobs fetched.", jobDtos);
        }
    }
}
