using Job_portal.Application.Features.JobApplications.Commands.ApplyJob;
using Job_portal.Application.Features.JobApplications.Commands.UpdateStatus;
using Job_portal.Application.Features.JobApplications.Queries.GetAppliedJobs;
using Job_portal.Application.Features.JobApplications.Queries.GetJobApplications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Job_portal.API.Controllers;

[Route("api/v1/application")]
[ApiController]
public class ApplicationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApplicationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // student views their applied jobs
    [HttpGet("get")]
    [Authorize(Policy = "StudentOnly")]
    public async Task<IActionResult> GetAppliedJobs()
    {
        var applicantId = Guid.Parse(
           User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(
            new GetAppliedJobsQuery { ApplicantId = applicantId });

        return Ok(new
        {
            success = result.Success,
            message = result.Message,
            application = result.Application // matches frontend useGetAppliedJobs
        });
    }

    // student applies for a job
    [HttpPost("apply/{jobId}")]
    [Authorize(Policy = "StudentOnly")]
    public async Task<IActionResult> ApplyJob(Guid jobId, [FromQuery] string? coverLetter)
    {
        var applicantId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new ApplyJobCommand
        {
            ApplicantId = applicantId,
            JobId = jobId,
            CoverLetter = coverLetter
        });

        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = result.Message, isApplied = true });
    }

    [HttpGet("{jobId}/applicants")]
    [Authorize(Policy = "RecruiterOnly")]
    public async Task<IActionResult> GetJobApplications(Guid jobId)
    {
        var result = await _mediator.Send(new GetJobApplicationsQuery
        {
            JobId = jobId,
            RecruiterId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!)
        });

        if (!result.Success)
            return NotFound(new { success = false, message = result.Message });

        // res.data.job → matches Applicants.jsx dispatch
        return Ok(new
        {
            success = result.Success,
            message = result.Message,
            job = result.Job
        });
    }

    // recruiter accepts or rejects applicant
    [HttpPost("status/{id}/update")]
    [Authorize(Policy = "RecruiterOnly")]
    public async Task<IActionResult> UpdateStatus(Guid id,[FromBody] UpdateStatusRequest request)
    {
        var result = await _mediator.Send(new UpdateStatusCommand
        {
            RecruiterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
            ApplicationId = id,
            Status = request.Status
        });

        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = result.Message });
    }
}

// Request Models 
public class UpdateStatusRequest
{
    public string Status { get; set; } = string.Empty;
}

