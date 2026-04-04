using AutoMapper;
using Job_portal.Application.Common.Interfaces;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand,UpdateCompanyResult>
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IFileService _fileService;

        public UpdateCompanyCommandHandler(IMapper mapper,ICompanyRepository companyRepository,IFileService fileService)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _fileService = fileService;
        }

        public async Task<UpdateCompanyResult> Handle(UpdateCompanyCommand request, CancellationToken ct)
        {
            //Fetch company — ownership check
            var company = await _companyRepository.GetByIdForRecruiterAsync(request.CompanyId,request.RecruiterId,ct);

            if (company == null) 
                return new UpdateCompanyResult(false, "Company not found.");

            //Update fields — only if provided
            if (!string.IsNullOrWhiteSpace(request.Name))
                company.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.Description))
                company.Description = request.Description;

            if (!string.IsNullOrWhiteSpace(request.Location))
                company.Location = request.Location;

            if (!string.IsNullOrWhiteSpace(request.WebSite))
                company.WebSite = request.WebSite;

            if(request.LogoFileName != null && request.LogoStream != null)
            {
                if (!string.IsNullOrWhiteSpace(company.Logo))
                    await _fileService.DeleteFileAsync(company.Logo);

                company.Logo = await _fileService.UploadImageAsync(request.LogoStream,request.LogoFileName,"logos");
            }

            await _companyRepository.SaveChangesAsync(ct);

            var companyDto = _mapper.Map<CompanyDto>(company);
            return new UpdateCompanyResult(true,"Company Updated.",companyDto);
        }
    }
}
