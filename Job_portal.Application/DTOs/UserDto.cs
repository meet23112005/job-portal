using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Application.DTOs
{
    public record UserDto
    {
        public Guid Id { get; init; }
        public string Fullname { get; init; } = string.Empty; 
        public string Email { get; init; } = string.Empty; 
        public string PhoneNumber { get; init; } = string.Empty; 
        public string Role { get; init; } = string.Empty; 
        public bool IsRemove { get; init; }                 
        public UserProfileDto? Profile { get; init; }
    }
}
