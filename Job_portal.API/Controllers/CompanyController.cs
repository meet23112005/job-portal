using Job_portal.Application.Features.Companies.Commands.CreateCompany;
using Job_portal.Application.Features.Companies.Commands.SoftDeleteCompany;
using Job_portal.Application.Features.Companies.Commands.UpdateCompany;
using Job_portal.Application.Features.Companies.Queries.GetAllCompanies;
using Job_portal.Application.Features.Companies.Queries.GetCompanyById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Job_portal.API.Controllers;

[Route("api/v1/company")]
[ApiController]
[Authorize]
public class CompanyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // recruiter sees their own companies
    [HttpGet("get")]
    [Authorize(Policy = "RecruiterOnly")]
    public async Task<IActionResult> GetAllCompanies()
    {
        var recruiterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(new GetAllCompaniesQuery { RecruiterId = recruiterId });
        return Ok(new
        {
            success = result.Success,
            message = result.Message,
            companies = result.Companies
        });
    }

    [HttpGet("get/{id}")]
    [Authorize]
    public async Task<IActionResult> GetCompanyById(Guid id)
    {
        var result = await _mediator.Send(new GetCompanyByIdQuery { CompanyId = id });
        if (!result.Success)
            return NotFound(new { success = false, message = result.Message });

        return Ok(new
        {
            success = result.Success,
            message = result.Message,
            company = result.Company
        });
    }

    [HttpPost("register")]
    [Authorize(Policy = "RecruiterOnly")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
    {
        var recruierId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(new CreateCompanyCommand
        {
            RecruiterId = recruierId,
            Name = request.CompanyName
        });

        if (!result.Success)
        {
            return BadRequest(new { success = false, message = result.Message });
        }

        return Ok(new
        {
            success = result.Success,
            message = result.Message,
            company = result.Company
        });
    }

    [HttpPut("update/{id}")]
    [Authorize(Policy = "RecruiterOnly")]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromForm] UpdateCompanyRequest request)
    {
        var recruiterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        Stream? logoStream = null;
        string? logoFileName = null;

        if (request.File != null && request.File.Length > 0)
        {
            logoStream = request.File.OpenReadStream();
            logoFileName = request.File.FileName;
        }

        var result = await _mediator.Send(new UpdateCompanyCommand
        {
            CompanyId = id,
            RecruiterId = recruiterId,
            Name = request.Name,
            Description = request.Description,
            WebSite = request.Website,
            Location = request.Location,
            LogoFileName = logoFileName,
            LogoStream = logoStream
        });

        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new
        {
            success = true,
            message = result.Message,
            company = result.Company
        });
    }

    // recruiter soft deletes their company
    [HttpPut("deleteCompany/{id}")]
    [Authorize(Policy = "RecruiterOnly")]
    public async Task<IActionResult> SoftDeleteCompany(Guid id)
    {
        var recruiterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new SoftDeleteCompanyCommand { RecruiterId = recruiterId,CompanyId = id });

        if(!result.Success) 
            return BadRequest(new { success = false, message = result.Message});

        return Ok(new { success = true, message = result.Message });
    }

}


//Request Models
public class CreateCompanyRequest
{
    public string CompanyName { get; set; } = string.Empty;
}

public class UpdateCompanyRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Website { get; set; }
    public string? Location { get; set; }
    public IFormFile? File { get; set; } // company logo
}