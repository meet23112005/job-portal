using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using Job_portal.Domain.Entities;
using MediatR;

namespace Job_portal.Application.Features.Jobs.Commands.CreateJob
{
    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, CreateJobResult>
    {
        private readonly IJobRepository _jobRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CreateJobCommandHandler(IJobRepository jobRepository,ICompanyRepository companyRepository, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<CreateJobResult> Handle(CreateJobCommand request, CancellationToken ct)
        {
            //Verify recruiter owns the company
            var company = await _companyRepository.GetByIdForRecruiterAsync(request.CompanyId,request.RecruiterId,ct);
            if (company == null)
                return new CreateJobResult(false, "Company not found or you don't own it.");

            var job = new Job
            {
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                ExperienceLevel = request.ExperienceLevel,
                Salary = request.Salary,
                Position = request.Position,
                Requirements = request.Requirements,
                JobType = request.JobType,
                CompanyId = request.CompanyId,
                CreatedBy = request.RecruiterId
            }; 

            //save
            _jobRepository.Add(job);
            await _jobRepository.SaveChangesAsync(ct);

            var jobDto = _mapper.Map<JobDto>(job);
            return new CreateJobResult(true, "Job Created Succesfully", jobDto);

        }
    }
}
