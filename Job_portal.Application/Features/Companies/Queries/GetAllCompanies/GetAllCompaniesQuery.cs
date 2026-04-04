using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Companies.Queries.GetAllCompanies
{
    // Returns recruiter's own companies
    // GET /api/v1/company/get
    public record GetAllCompaniesQuery : IRequest<GetAllCompaniesResult>
    {
        public Guid RecruiterId { get; init; }    //from JWT Token.
    }
    public record GetAllCompaniesResult(bool Success, string Message, IEnumerable<CompanyDto>? Companies = null);
}
