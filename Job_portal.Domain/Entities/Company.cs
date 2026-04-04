namespace Job_portal.Domain.Entities
{
    public class Company:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? WebSite {  get; set; }
        public string? Location { get; set; }
        public string? Logo { get; set; }
        public bool IsRemoved { get; set; } = false;
        public Guid CreatedBy { get; set; }

        public User? Creator { get; set; }

        public ICollection<Job> Jobs { get; set; } = new List<Job>();

    }
}
