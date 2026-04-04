using MediatR;

namespace Job_portal.Application.Features.Admin.Commands.AdminLogin
{
    // POST /api/v1/admin/login
    // admin logs in with hardcoded credentials from appsettings
    public record AdminLoginCommand : IRequest<AdminLoginResult>
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
    public record AdminLoginResult(bool Success, string Message, string? Token = null);
}
