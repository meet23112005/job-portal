using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Jobs.Queries.GetJobById
{
    public class GetJobByIdQueryHandler : IRequestHandler<GetJobByIdQuery,GetByJobIdResult>
    {
        private readonly IJobRepository _jobRepo;
        private readonly IMapper _mapper;

        public GetJobByIdQueryHandler(IJobRepository jobRepo , IMapper mapper)
        {
            _jobRepo = jobRepo;
            _mapper = mapper;
        }

        public async Task<GetByJobIdResult> Handle(GetJobByIdQuery request, CancellationToken ct)
        {
            // includes Company + Applications for full detail
            var job = await _jobRepo.GetByIdWithCompanyAsync(request.JobId,ct);
            if(job == null)
                return new GetByJobIdResult(false,"Job Not Found");

            var jobDto = _mapper.Map<JobDto>(job);
            return new GetByJobIdResult(true,"Job Fetched By Id.",jobDto);
        }
    }
}
