using Job_portal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Domain.Entities
{
    public class User:BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; }
        public bool IsRemoved { get; set; } = false;

        public UserProfile? Profile { get; set; }

        public ICollection<Company> Companies { get; set; } = new List<Company>();
        public ICollection<Job> Jobs { get; set; } = new List<Job>();

        // Applications submitted by this user
        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

        // Saved jobs
        public ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
    }
}
