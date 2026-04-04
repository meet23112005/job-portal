using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Admin.Queries.GetAllUsers
{
    // GET /api/v1/user/getAllUser
    // admin sees all users
    public record GetAllUserQuery : IRequest<GetAllUserResult>;
    
    public record GetAllUserResult(bool Success, string Message, IEnumerable<UserDto>? Users = null);
}
