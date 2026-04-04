using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Jobs.Queries.GetAllJobs
{
    public class GetAllJobsQueryHandler : IRequestHandler<GetAllJobsQuery,GetAllJobsResult>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;

        public GetAllJobsQueryHandler(IJobRepository jobRepository,IMapper mapper)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
        }

        public async Task<GetAllJobsResult> Handle(GetAllJobsQuery request, CancellationToken ct)
        {
            var (jobs,totalCount) = await _jobRepository.GetPaginatedAsync(request.Filter,request.PageNumber,request.PageSize, ct);

            var jobDtos = _mapper.Map<IEnumerable<JobDto>>(jobs);
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new GetAllJobsResult(true,"all jobs fetched.", jobDtos, totalCount, request.PageNumber, totalPages);

        }
    }
}
