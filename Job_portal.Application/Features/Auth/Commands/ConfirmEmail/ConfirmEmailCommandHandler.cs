using Job_portal.Application.Common.Interfaces.Repositories;
using MediatR;

namespace Job_portal.Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ConfirmEmailResult>
    {
        private readonly IUserRepository _userRepository;

        public ConfirmEmailCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ConfirmEmailResult> Handle(ConfirmEmailCommand request, CancellationToken ct)
        {
            var user = await _userRepository.GetByConfirmationTokenAsync(request.Token, ct);

            if (user == null) 
                return new ConfirmEmailResult(false,"Invalid Confirmation link.");

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;

            await _userRepository.SaveChangesAsync(ct);

            return new ConfirmEmailResult(true, "Email confirmed successfully.");
        }
    }
}
