using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Companies.Queries.GetCompanyById
{
    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery,GetCompanyByIdResult>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public GetCompanyByIdQueryHandler(ICompanyRepository companyRepository,IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<GetCompanyByIdResult> Handle(GetCompanyByIdQuery request, CancellationToken ct)
        {
            // anyone can view ,no need of ownership check 
            var company = await _companyRepository.GetByIdAsync(request.CompanyId,ct);
            if (company == null)
                return new GetCompanyByIdResult(false, "Company not found.");

            var companyDto = _mapper.Map<CompanyDto>(company);
            return new GetCompanyByIdResult(true,"Company fetched",companyDto);
        }
    }
}
