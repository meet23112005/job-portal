using Job_portal.Application.Features.Admin.Commands.RemoveUser;
using Job_portal.Application.Features.Admin.Queries.GetAllJobSeekers;
using Job_portal.Application.Features.Admin.Queries.GetAllRecruiters;
using Job_portal.Application.Features.Admin.Queries.GetAllUsers;
using Job_portal.Application.Features.Auth.Commands.ConfirmEmail;
using Job_portal.Application.Features.Auth.Commands.ForgotPassword;
using Job_portal.Application.Features.Auth.Commands.Login;
using Job_portal.Application.Features.Auth.Commands.Register;
using Job_portal.Application.Features.Auth.Commands.ResetPassword;
using Job_portal.Application.Features.Auth.Commands.UpdateProfileCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Job_portal.API.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //POST /api/v1/user/register 
        // no auth required — public endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            Stream? FileStream = null;
            string? FileName = null;
            if (request.File != null && request.File.Length > 0)
            {
                FileStream = request.File.OpenReadStream();
                FileName = request.File.FileName;
            }

            var result = await _mediator.Send(new RegisterCommand
            {
                FullName = request.FullName,
                Email = request.Email,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
                PhotoFileName = FileName,
                PhotoStream = FileStream,
                Role = request.Role,
            });

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new
            {
                success = true,
                message = result.Message,
            });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            var result = await _mediator.Send(new ConfirmEmailCommand
            {
                Token = token
            });

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            // Redirect to login page after confirmation
            return Redirect("http://localhost:5173/login?confirmed=true");
        }

        // POST /api/v1/user/forgot-password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _mediator.Send(new ForgotPasswordCommand
            {
                Email = request.Email
            });

            // always return 200 — don't reveal if email exists
            return Ok(new { success = true, message = result.Message });
        }

        // POST /api/v1/user/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _mediator.Send(new ResetPasswordCommand
            {
                Token = request.Token,
                NewPassword = request.NewPassword,
                ConfirmPassword = request.ConfirmPassword
            });

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, message = result.Message });
        }


        // no auth required — public endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _mediator.Send(new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            });

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            // set JWT token in HttpOnly cookie
            SetTokenCookie(result.Token!);

            return Ok(new
            {
                success = true,
                message = result.Message,
                token = result.Token,
                user = result.User
            });
        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            // clear HttpOnly cookie
            Response.Cookies.Delete("token");
            return Ok(new { success = true, message = "Logged out successfully." });
        }

        // ── POST /api/v1/user/profile/update ──────────────
        // requires auth — student or recruiter
        [HttpPut("profile/update")]
        [Authorize(Policy = "StudentOrRecruiter")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileRequest request)
        {
            var userId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            Stream? photoStream = null;
            string? photoFileName = null;
            Stream? resumeStream = null;
            string? resumeFileName = null;
            string? resumeOriginalName = null;

            if (request.File != null && request.File.Length > 0)
            {
                photoStream = request.File.OpenReadStream();
                photoFileName = request.File.FileName;
            }
            if (request.Resume != null && request.Resume.Length > 0)
            {
                resumeStream = request.Resume.OpenReadStream();
                resumeFileName = request.Resume.FileName;
                resumeOriginalName = request.Resume.FileName;
            }
            var result = await _mediator.Send(new UpdateProfileCommand
            {
                UserId = userId,
                FullName = request.FullName,
                Bio = request.Bio,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Skills = !string.IsNullOrEmpty(request.Skills)
                            ? request.Skills.Split(',')
                                            .Select(s => s.Trim())
                                            .Where(s => !string.IsNullOrEmpty(s))
                                            .ToList()
                            : null,
                PhotoFileName = photoFileName,
                PhotoStream = photoStream,
                ResumeFileName = resumeFileName,
                ResumeStream = resumeStream,
                ResumeOriginalName = resumeOriginalName
            });

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new
            {
                success = true,
                message = result.Message,
                user = result.User
            });
        }

        //  GET /api/v1/user/getAllUser 
        // admin only
        [HttpGet("getAllUser")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _mediator.Send(new GetAllUserQuery());
            return Ok(new
            {
                success = result.Success,
                message = result.Message,
                users = result.Users
            });
        }

        // GET /api/v1/user/getAllRecruiter
        // admin only
        [HttpGet("getAllRecruiter")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllRecruiters()
        {
            var result = await _mediator.Send(new GetAllRecruitersQuery());

            return Ok(new
            {
                success = result.Success,
                message = result.Message,
                users = result.Users
            });
        }

        // GET /api/v1/user/getAllJobSeeker 
        // admin only
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("getAllJobSeeker")]
        public async Task<IActionResult> GetAllJobSeekers()
        {
            var result = await _mediator.Send(new GetAllJobSeekersQuery());

            return Ok(new
            {
                success = result.Success,
                message = result.Message,
                users = result.Users
            });
        }

        // DELETE /api/v1/user/removeUser/{id} 
        // admin only
        //sets JWT in HttpOnly cookie
        [HttpDelete("removeuser/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _mediator.Send(new RemoveUserCommand
            {
                UserId = id
            });
            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(new { success = true, message = result.Message });
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,//set true in production
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            };
            Response.Cookies.Append("token", token, cookieOptions);
        }

    }
    // these live in API layer — IFormFile is ok here 
    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public IFormFile? File { get; set; } // profile photo
    }
    public class UpdateProfileRequest
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public string? Skills { get; set; }
        public IFormFile? File { get; set; } // profile photo
        public IFormFile? Resume { get; set; } // resume PDF
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordRequest
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
