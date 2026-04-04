using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Companies.Queries.GetCompanyById
{
    // Returns single company detail
    // GET /api/v1/company/get/{id}
    public record GetCompanyByIdQuery : IRequest<GetCompanyByIdResult>
    {
        public Guid CompanyId { get; init; } 
    }
    public record GetCompanyByIdResult(bool Success,string Message,CompanyDto? Company = null);
}
