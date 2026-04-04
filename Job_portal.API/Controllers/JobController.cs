using Job_portal.Application.Features.Jobs.Commands.CreateJob;
using Job_portal.Application.Features.Jobs.Commands.DeleteJob;
using Job_portal.Application.Features.Jobs.Commands.UpdateJob;
using Job_portal.Application.Features.Jobs.Queries.GetAdminJobs;
using Job_portal.Application.Features.Jobs.Queries.GetAllJobs;
using Job_portal.Application.Features.Jobs.Queries.GetJobById;
using Job_portal.Application.Features.SavedJobs.Commands.SaveJob;
using Job_portal.Application.Features.SavedJobs.Commands.UnsaveJob;
using Job_portal.Application.Features.SavedJobs.Queries.GetSavedJobs;
using Job_portal.Application.Features.SavedJobs.Queries.GetSaveStatus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace Job_portal.API.Controllers;

[Route("api/v1/job")]
[ApiController]
public class JobController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /api/v1/job/get?keyword=&location=&jobType=&pageNumber=&pageSize=
    // public — student browses jobs
    [HttpGet("get")]
    public async Task<IActionResult> GetAllJobs([FromQuery] GetAllJobsRequest request)
    {
        var result = await _mediator.Send(new GetAllJobsQuery
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            Filter = new Application.Common.Models.JobFilter
            {
                CompanyId = request.CompanyId,
                Keyword = request.Keyword,
                Location = request.Location,
                JobType = request.JobType,
                ExperienceLevel = request.ExperienceLevel,
                MaxSalary = request.MaxSalary,
                MinSalary = request.MinSalary
            }
        });
        return Ok(new
        {
            success = result.Success,
            message = result.Message,
            jobs = result.Jobs,
            total = result.TotalCount,
            pageNumber = result.PageNumber,
            totalPages = result.TotalPages
        });
    }

    [HttpGet("get/{jobId}")]
    public async Task<IActionResult> GetJobById(Guid jobId)
    {
        var result = await _mediator.Send(
            new GetJobByIdQuery { JobId = jobId });

        if (!result.Success)
            return NotFound(new { success = false, message = result.Message });

        return Ok(new
        {
            success = result.Success,
            message = result.Message,
            job = result.Job
        });
    }

    // recruiter sees their own posted jobs
    [HttpGet("getadminjobs")]
    [Authorize(Policy = "RecruiterOnly")]
    public async Task<IActionResult> GetAdminJobs()
    {
        var recruiterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(
            new GetAdminJobsQuery { RecruiterId = recruiterId });

        return Ok(new
        {
            success = result.Success,
            message = result.Message,
            jobs = result.Jobs
        });
    }

    [HttpPost("post")]
    [Authorize(Policy = "RecruiterOnly")]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobRequest request)
    {
        var recruiterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new CreateJobCommand
        {
            RecruiterId = recruiterId,
            Title = request.Title,
            Description = request.Description,
            Requirements = request.Requirements,
            Salary = request.Salary,
            Location = request.Location,
            JobType = request.JobType,
            ExperienceLevel = request.ExperienceLevel,
            Position = request.Position,
            CompanyId = request.CompanyId
        });

        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new
        {
            success = true,
            message = result.Message,
            job = result.Job
        });
    }

    // recruiter updates their job
    [HttpPut("job/{id}")]
    [Authorize(Policy = "RecruiterOnly")]
    public async Task<IActionResult> UpdateJob(Guid id, [FromBody] UpdateJobRequest request)
    {
        var recruiterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(new UpdateJobCommand
        {
            JobId = id,
            RecruiterId = recruiterId,
            Title = request.Title,
            Description = request.Description,
            JobType = request.JobType,
            ExperienceLevel = request.ExperienceLevel,
            Location = request.Location,
            Position = request.Position,
            Requirenments = request.Requirements,
            Salary = request.Salary
        });

        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new
        {
            success = true,
            message = result.Message,
            job = result.Job
        });
    }

    // recruiter soft deletes — admin hard deletes
    [HttpDelete("DeleteJob/{id}")]
    [Authorize(Policy = "RecruiterOnly")]
    public async Task<IActionResult> DeleteJob(Guid id)
    {
        var recruiterId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var role = User.FindFirstValue(ClaimTypes.Role);
        var isAdmin = role?.ToLower() == "admin";

        var result = await _mediator.Send(new DeleteJobCommand
        {
            JobId = id,
            RecruiterId = recruiterId,
            IsAdmin = isAdmin
        });

        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = result.Message });
    }

    [HttpGet("{jobId}/save-status")]
    [Authorize(Policy = "StudentOnly")]
    public async Task<IActionResult> GetSaveStatus(Guid jobId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new GetSaveStatusQuery { JobId = jobId, UserId = userId });
        return Ok(new { success = result.Success, isSaved = result.IsSaved }); 
    }

    [HttpGet("saved")]
    [Authorize(Policy = "StudentOnly")]
    public async Task<IActionResult> GetSavedJobs()
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(new GetSavedJobsQuery { UserId = userId });
        return Ok(new
        {
            success = result.Success,
            message = result.Message,
            savedJobs = result.Jobs
        });
    }

    [HttpPost("saved-jobs/{jobId}")]
    [Authorize(Policy= "StudentOnly")]
    public async Task<IActionResult> SaveJob(Guid jobId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(new SaveJobCommand { JobId = jobId, UserId = userId });

        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = result.Message });
    }
    [HttpPost("{jobId}/save")]
    [Authorize(Policy = "StudentOnly")]
    public async Task<IActionResult> SavedJob(Guid jobId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(new SaveJobCommand { JobId = jobId, UserId = userId });

        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = result.Message });
    }

    [HttpDelete("unsave/{jobId}")]
    [Authorize(Policy= "StudentOnly")]
    public async Task<IActionResult> UnsaveJob(Guid jobId)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        var result = await _mediator.Send(new UnsaveJobCommand { JobId = jobId,UserId = userId });
        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = result.Message });
    }

}

//request models
public class GetAllJobsRequest
{
    public string? Keyword { get; set; }
    public string? Location { get; set; }
    public string? JobType { get; set; }
    public string? ExperienceLevel { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public Guid? CompanyId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class CreateJobRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public string Location { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
    public string ExperienceLevel { get; set; } = string.Empty;
    public int Position { get; set; }
    public Guid CompanyId { get; set; }
}

public class UpdateJobRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Requirements { get; set; }
    public decimal? Salary { get; set; }
    public string? Location { get; set; }
    public string? JobType { get; set; }
    public string? ExperienceLevel { get; set; }
    public int? Position { get; set; }
}