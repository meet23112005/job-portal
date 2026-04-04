using MediatR;

namespace Job_portal.Application.Features.Companies.Commands.SoftDeleteCompany
{
    public record SoftDeleteCompanyCommand : IRequest<SoftDeleteCompanyResult>
    {
        public Guid CompanyId { get; init; }
        public Guid RecruiterId { get; init; } // from JWT token
    }
    public record SoftDeleteCompanyResult(bool Success, string Message);
}
