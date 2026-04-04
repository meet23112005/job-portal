using Job_portal.Application.Features.Admin.Commands.AdminLogin;
using Job_portal.Application.Features.Admin.Queries.GetAllAdminCompanies;
using Job_portal.Application.Features.Admin.Queries.GetJobWithapplicants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Job_portal.API.Controllers;

[Route("api/v1/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginRequest request)
    {
        var result = await _mediator.Send(new AdminLoginCommand
        {
            Email = request.Email,
            Password = request.Password
        });

        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        // set admin JWT in HttpOnly cookie
        Response.Cookies.Append("token", result.Token!, new CookieOptions
        {
            HttpOnly = true,
            Secure = false, //set to true in prod
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(1)
        });

        return Ok(new
        {
            success = true,
            message = result.Message,
            token = result.Token
        });
    }

    [HttpGet("{jobId}/applicants")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetJobWithApplicants(Guid jobId)
    {
        var result = await _mediator.Send(new GetJobWithApplicantsQuery
        {
            JobId=jobId
        });

        if (!result.Success)
            return NotFound(new { success = false, message = result.Message });

        return Ok(new
        {
            success = result.Success,
            message = result.Message,
            job = result.Job
        });
    }

    [HttpGet("getAllCompanies")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAllCompanies()
    {
        var result = await _mediator.Send(new GetAllAdminCompaniesQuery { });
        return Ok(new
        {
            success = result.Success,
            message = result.Message,
            companies = result.Companies
        });
    }
}

// Request Models 
public class AdminLoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
