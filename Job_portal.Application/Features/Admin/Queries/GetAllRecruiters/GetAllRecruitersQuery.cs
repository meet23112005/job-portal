using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Admin.Queries.GetAllRecruiters
{
    // admin sees all recruiters
    public record GetAllRecruitersQuery : IRequest<GetAllRecruitersResult>;

    public record GetAllRecruitersResult(bool Success, string Message, IEnumerable<UserDto>? Users = null);
}
