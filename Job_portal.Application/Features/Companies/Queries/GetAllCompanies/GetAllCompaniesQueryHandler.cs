using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Companies.Queries.GetAllCompanies
{
    public class GetAllCompaniesQueryHandler: IRequestHandler<GetAllCompaniesQuery,GetAllCompaniesResult>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public GetAllCompaniesQueryHandler(ICompanyRepository companyRepository,IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<GetAllCompaniesResult> Handle(GetAllCompaniesQuery request, CancellationToken ct)
        {
            //filtered by RecruiterId
            //returns recruiter's own companies only
            var companies = await _companyRepository.GetByRecruiterAsync(request.RecruiterId,ct);

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return new GetAllCompaniesResult(true, "Companies fetched.", companiesDto);
        }
    }
}
