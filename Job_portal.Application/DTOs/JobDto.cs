namespace Job_portal.Application.DTOs
{
    public record JobDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Requirements { get; init; } = string.Empty; 
        public decimal Salary { get; init; }               
        public string Location { get; init; } = string.Empty; 
        public string JobType { get; init; } = string.Empty; 
        public string ExperienceLevel { get; init; } = string.Empty; 
        public int Position { get; init; }               
        public int ApplicationCount { get; init; } 
        public CompanyDto? Company { get; init; }  
        public DateTime CreatedAt { get; init; }
    }
}
