using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.JobApplications.Queries.GetAppliedJobs
{
    public class GetAppliedJobsQueryHandler:IRequestHandler<GetAppliedJobsQuery, GetAppliedJobsResult>
    {
        private readonly IJobApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;

        public GetAppliedJobsQueryHandler(IJobApplicationRepository applicationRepository,IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        public async Task<GetAppliedJobsResult> Handle(GetAppliedJobsQuery request, CancellationToken ct)
        {
            // includes Job + Company data
            var appliedJob = await _applicationRepository.GetByApplicantAsync(request.ApplicantId,ct);

            var appliedJobDto = _mapper.Map<IEnumerable<JobApplicationDto>>(appliedJob);
            return new GetAppliedJobsResult(true, "Applications fetched.", appliedJobDto);
        }
    }
}
