using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.Features.Admin.Queries.GetAllJobs
{
    internal class GetAllAdminJobsQueryHandler : IRequestHandler<GetAllAdminJobsQuery, GetAllAdminJobsResult>
    {
        private readonly IJobRepository jobRepository;
        private readonly IMapper mapper;

        public GetAllAdminJobsQueryHandler(IJobRepository jobRepository,IMapper mapper)
        {
            this.jobRepository = jobRepository;
            this.mapper = mapper;
        }

        public async Task<GetAllAdminJobsResult> Handle(GetAllAdminJobsQuery request, CancellationToken ct)
        {
            var jobs = await jobRepository.GetAllForAdminAsync(ct);

            var jobDtos = mapper.Map<IEnumerable<JobDto>>(jobs);

            return new GetAllAdminJobsResult(true, "all jobs fetched.", jobDtos);
        }
    }
}
