using Job_portal.Application.Common.Models;
using Job_portal.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.Features.Admin.Queries.GetAllJobs
{
    public record GetAllAdminJobsQuery() : IRequest<GetAllAdminJobsResult>;

    public record GetAllAdminJobsResult(bool Success, string Message, IEnumerable<JobDto>? Jobs = null);
}
