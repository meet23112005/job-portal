using Job_portal.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.Features.Auth.Commands.Login
{
    public record LoginCommand : IRequest<LoginResult>
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
    public record LoginResult(bool Success, string Message, string? Token = null, UserDto? User = null);
}
