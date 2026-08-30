namespace Job_portal.Application.DTOs
{
    public record CompanyDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty; 
        public string? Description { get; init; }         
        public string? Website { get; init; }             
        public string? Location { get; init; }            
        public string? Logo { get; init; }                
      //  public bool IsRemove { get; init; }  //removed as not required in frontend             
        public DateTime CreatedAt { get; init; }          

    }
}
