using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.DTOs
{
    public record ApplicationWithApplicantDto
    {
        public Guid Id { get; init; }
        public string Status { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
        public ApplicantDto? Applicant { get; init; } // optional → might not always include
    }
}
