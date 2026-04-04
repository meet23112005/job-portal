using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.SavedJobs.Queries.GetSavedJobs
{
    public class GetSavedJobsQueryHandler : IRequestHandler<GetSavedJobsQuery, GetSavedJobsResult>
    {
        private readonly ISavedJobRepository _savedJobRepository;
        private readonly IMapper _mapper;

        public GetSavedJobsQueryHandler(ISavedJobRepository savedJobRepository, IMapper mapper)
        {
            _savedJobRepository = savedJobRepository;
            _mapper = mapper;
        }

        public async Task<GetSavedJobsResult> Handle(GetSavedJobsQuery request, CancellationToken ct)
        {
            // includes Job + Company data
            var savedJobs = await _savedJobRepository.GetByUserAsync(request.UserId, ct);

            var jobs = savedJobs.Select(x => x.Job);

            //var savedJobDtos = _mapper
            //   .Map<IEnumerable<SavedJobDto>>(savedJobs);

            var jobDtos = _mapper
               .Map<IEnumerable<JobDto>>(jobs);
            return new GetSavedJobsResult(
                true, "Saved jobs fetched.", jobDtos);
        }
    }
}
