using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.DTOs
{
    public record UserProfileDto
    {
        public string? ProfilePhoto { get; init; }  
        public string? Resume { get; init; } 
        public string? ResumeOriginalName { get; init; }  
        public string? Bio { get; init; } 
        public List<string> Skills { get; init; } = new();
    }
}
