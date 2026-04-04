using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.DTOs
{
    public record JobApplicationDto
    {
        public Guid Id { get; init; }
        public string Status { get; init; } = string.Empty;
        public string? CoverLetter { get; init; }          
        public JobDto? Job { get; init; }                 
        public DateTime CreatedAt { get; init; }
    }
}
