using Job_portal.Application.Common.Interfaces.Repositories;
using MediatR;

namespace Job_portal.Application.Features.Admin.Commands.RemoveUser
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand,RemoveUserResult>
    {
        private readonly IUserRepository _userRepository;

        public RemoveUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RemoveUserResult> Handle(RemoveUserCommand request, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, ct);
            if (user == null)
                return new RemoveUserResult(false, "User Not Found.");

            // 2. Hard delete — removes from DB completely
            _userRepository.Remove(user);
            await _userRepository.SaveChangesAsync(ct);

            return new RemoveUserResult(true, "User Succesfully Removed.");
        }
    }
}
