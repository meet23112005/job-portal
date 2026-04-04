using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using Job_portal.Domain.Entities;
using MediatR;

namespace Job_portal.Application.Features.Companies.Commands.CreateCompany
{
    
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CreateCompanyResult>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CreateCompanyCommandHandler(ICompanyRepository companyRepository,IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<CreateCompanyResult> Handle(CreateCompanyCommand request, CancellationToken ct)
        {
            var companyExists =await _companyRepository.ExistsByNameForRecruiterAsync(request.Name,request.RecruiterId ,ct);

            if (companyExists)
                return new CreateCompanyResult(false, "You already have a company with this name.");

            var Company = new Company
            {
                Name = request.Name,
                CreatedBy = request.RecruiterId
            };

            //save
            _companyRepository.Add(Company);
            await _companyRepository.SaveChangesAsync(ct);

            //map and return
            var companyDto = _mapper.Map<CompanyDto>(Company);
            return new CreateCompanyResult(true, "Company registered.", companyDto);
        }
    }
}
