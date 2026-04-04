using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Admin.Queries.GetAllAdminCompanies
{
    public class GetAllAdminCompaniesQueryHandler : IRequestHandler<GetAllAdminCompaniesQuery, GetAllAdminCompaniesResult>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public GetAllAdminCompaniesQueryHandler(ICompanyRepository companyRepository,IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<GetAllAdminCompaniesResult> Handle(GetAllAdminCompaniesQuery request, CancellationToken ct)
        {

            var companies = await _companyRepository.GetAllAsync(ct);
            var companiesDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return new GetAllAdminCompaniesResult(true, "Recruiters fetched.", companiesDtos);
        }
    }
}
