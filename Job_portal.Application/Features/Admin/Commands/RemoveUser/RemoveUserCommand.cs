using MediatR;

namespace Job_portal.Application.Features.Admin.Commands.RemoveUser
{
    // DELETE /api/v1/user/removeUser/{id}
    // admin hard deletes a user
    public record RemoveUserCommand : IRequest<RemoveUserResult>
    {
        public Guid UserId { get; init; }
    }
    public record RemoveUserResult(bool Success, string Message);
}
