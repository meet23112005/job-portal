using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Admin.Queries.GetAllAdminCompanies

{
    // admin sees all recruiters
    public record GetAllAdminCompaniesQuery : IRequest<GetAllAdminCompaniesResult>;

    public record GetAllAdminCompaniesResult(bool Success, string Message, IEnumerable<CompanyDto>? Companies = null);
}
