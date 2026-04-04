using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Companies.Commands.UpdateCompany
{
    public record UpdateCompanyCommand:IRequest<UpdateCompanyResult>
    {
        public Guid CompanyId { get; init; }
        public Guid RecruiterId { get; init; } // from JWT token
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? WebSite { get; init; }
        public string? Location { get; init; }
        public Stream? LogoStream {  get; init; }
        public string? LogoFileName { get; init; }

    }
    public record UpdateCompanyResult(
        bool Success,
        string Message,
        CompanyDto? Company = null);
}
