using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Admin.Queries.GetAllJobSeekers
{
    public record GetAllJobSeekersQuery : IRequest<GetAllJobSeekersResult>;
    public record GetAllJobSeekersResult(
        bool Success,
        string Message,
        IEnumerable<UserDto>? Users = null);
}
