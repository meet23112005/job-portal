using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.DTOs
{
    public record ApplicantDto
    {
        public Guid Id { get; init; }
        public string Fullname { get; init; } = string.Empty; 
        public string Email { get; init; } = string.Empty; 
        public string PhoneNumber { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }                 
        public UserProfileDto? Profile { get; init; }              
    }
}
