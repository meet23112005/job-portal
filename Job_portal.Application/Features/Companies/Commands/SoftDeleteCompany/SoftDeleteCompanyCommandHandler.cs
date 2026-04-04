using Job_portal.Application.Common.Interfaces.Repositories;
using MediatR;

namespace Job_portal.Application.Features.Companies.Commands.SoftDeleteCompany
{
    public class SoftDeleteCompanyCommandHandler : IRequestHandler<SoftDeleteCompanyCommand,SoftDeleteCompanyResult>
    {
        private readonly ICompanyRepository _companyRepository;

        public SoftDeleteCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<SoftDeleteCompanyResult> Handle(SoftDeleteCompanyCommand request, CancellationToken ct)
        {
            var company = await _companyRepository.GetByIdForRecruiterAsync(request.CompanyId,request.RecruiterId,ct);
            if (company == null) 
                return new SoftDeleteCompanyResult(false,"Company Not Found.");

            //Soft delete — just set flag
            // company stays in DB — jobs still work
            //company just hidden from listings
            company.IsRemoved = true;
            await _companyRepository.SaveChangesAsync(ct);

            return new SoftDeleteCompanyResult(true,"Company Removed.");
        }
    }
}
