namespace Job_portal.Domain.Entities
{
    public class Job : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty; // ← ADD
        public decimal Salary { get; set; }
        public string Location { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;
        public string ExperienceLevel { get; set; } = string.Empty;
        public int Position { get; set; }             
        public bool IsRemoved { get; set; } = false;

        // ── Foreign Keys ──────────────────────────────────
        public Guid CompanyId { get; set; }
        public Guid CreatedBy { get; set; }  

        // ── Navigation Properties ─────────────────────────
        public Company? Company { get; set; }
        public User? Creator { get; set; }  

        public ICollection<JobApplication> Applications { get; set; }
            = new List<JobApplication>();

        public ICollection<SavedJob> SavedJobs { get; set; }  
            = new List<SavedJob>();
    }
}
