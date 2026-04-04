using Job_portal.Application.Common.Interfaces;
using Job_portal.Application.Common.Settings;
using MediatR;
using Microsoft.Extensions.Options;



namespace Job_portal.Application.Features.Admin.Commands.AdminLogin
{
    public class AdminLoginCommandHandler : IRequestHandler<AdminLoginCommand,AdminLoginResult>
    {
        private readonly IJwtServiceAdmin _jwtServiceAdmin;
        private readonly AdminSettings _adminSettings;

        public AdminLoginCommandHandler(IJwtServiceAdmin jwtServiceAdmin, IOptions<AdminSettings> adminSettings)
        {
            _jwtServiceAdmin = jwtServiceAdmin;
            _adminSettings = adminSettings.Value;
        }

        public async Task<AdminLoginResult> Handle(AdminLoginCommand request, CancellationToken ct)
        {
            //  strongly typed — no magic strings
            if (request.Email != _adminSettings.Email || request.Password != _adminSettings.Password)
                return new AdminLoginResult(false, "Invalid credentials.");

            var token = _jwtServiceAdmin.GenerateAdminToken(request.Email);
            return new AdminLoginResult(true, "Admin login successful.", token);

        }
    }
}
