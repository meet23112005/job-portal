using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.Features.SavedJobs.Queries.GetSaveStatus
{
    public record GetSaveStatusQuery : IRequest<GetSaveStatusResult>
    {
        public Guid JobId { get; set; }
        public Guid UserId { get; set; }
    }
    public record GetSaveStatusResult(bool Success, string Message, bool? IsSaved = false);
}
