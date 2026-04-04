using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Companies.Commands.CreateCompany
{
    public record CreateCompanyCommand : IRequest<CreateCompanyResult>
    {
        public string Name { get; init; } = string.Empty;
        public Guid RecruiterId { get; init; } // From JWT Token. 
    }
    public record CreateCompanyResult(bool Success, string Message,CompanyDto? Company = null);
}
