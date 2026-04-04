using System;
using System.Collections.Generic;
using System.Text;

namespace Job_portal.Domain.Entities
{
    public class SavedJob:BaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid JobId { get; set; }
        public Job? Job { get; set; }
    }
}
