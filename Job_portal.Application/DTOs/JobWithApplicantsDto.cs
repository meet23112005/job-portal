using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.DTOs
{
    public record JobWithApplicantsDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public List<ApplicationWithApplicantDto> Applications { get; init; } = new();
    }
}
