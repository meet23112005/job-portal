using Job_portal.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Domain.Entities
{
    public  class JobApplication:BaseEntity
    {
        public Guid JobId { get; set; }
        public Job? Job { get; set; }

        public Guid ApplicantId { get; set; }
        public User? Applicant { get; set; }

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        public string? CoverLetter { get; set; }  // ← ADD THIS

    }
}
