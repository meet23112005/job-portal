using Job_portal.Application.Common.Models;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Jobs.Queries.GetAllJobs
{
    // GET /api/v1/job/get
    // student browses all jobs with filters + pagination
    public record GetAllJobsQuery : IRequest<GetAllJobsResult>
    {
        public JobFilter Filter { get; init; } = new();
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
    public record GetAllJobsResult(
        bool Success, 
        string Message, 
        IEnumerable<JobDto>? Jobs = null,
        int TotalCount = 0, 
        int PageNumber=1,
        int TotalPages = 0);
}
