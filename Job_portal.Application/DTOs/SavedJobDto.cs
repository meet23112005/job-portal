using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.DTOs
{
    public record SavedJobDto
    {
        public Guid Id { get; init; }
        public JobDto? Job { get; init; } 
        public DateTime CreatedAt { get; init; }
    }
}
