using AutoMapper;
using Job_portal.Application.DTOs;
using Job_portal.Domain.Entities;

namespace Job_portal.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            #region User Mapping
            //user
            CreateMap<User, UserDto>()
                .ForMember(d => d.Fullname,
                    o => o.MapFrom(s => s.FullName))
                .ForMember(d => d.IsRemove,
                    o => o.MapFrom(s => s.IsRemoved))
                .ForMember(d => d.Role,
                    o => o.MapFrom(s => s.Role.ToString().ToLower()));

            CreateMap<UserProfile, UserProfileDto>()
                .ForMember(d => d.Resume,
                    o => o.MapFrom(s => s.ResumePath));
            #endregion

            //Company
            CreateMap<Company, CompanyDto>()
                .ForMember(d => d.IsRemove,
                    o => o.MapFrom(s => s.IsRemoved));

            //Job
            CreateMap<Job, JobDto>()
                .ForMember(d => d.ApplicationCount,
                    o => o.MapFrom(s => s.Applications != null ? s.Applications.Count :0));

            //jobApplication
            CreateMap<JobApplication, JobApplicationDto>().
                ForMember(d => d.Status,
                    o => o.MapFrom(s => s.Status.ToString().ToLower()));

            #region Job With Applicants
            CreateMap<Job, JobWithApplicantsDto>();

            CreateMap<JobApplication, ApplicationWithApplicantDto>()
                .ForMember(d => d.Status,
                    o => o.MapFrom(s => s.Status.ToString().ToLower()));

            CreateMap<User, ApplicantDto>()
               .ForMember(d => d.Fullname,
                   o => o.MapFrom(s => s.FullName));
            #endregion

            // Saved Job 
            CreateMap<SavedJob, SavedJobDto>();
        }
    }
}
