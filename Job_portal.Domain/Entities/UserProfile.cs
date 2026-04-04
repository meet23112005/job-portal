using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Domain.Entities
{
    public class UserProfile:BaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public string? ProfilePhoto { get; set; }
        public string? ResumePath { get; set; }             // stored URL/path
        public string? ResumeOriginalName { get; set; }     // original filename — frontend: profile.resumeOriginalName
        public string? Bio { get; set; }
        public List<string> Skills { get; set; } = new();
    }
}
