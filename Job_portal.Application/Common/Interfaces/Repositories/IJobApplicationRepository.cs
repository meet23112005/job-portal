using Job_portal.Domain.Entities;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Job_portal.Application.Common.Interfaces.Repositories
{
    public interface IJobApplicationRepository
    {
        // GET /api/v1/application/status/{id}/update — basic existence check
        Task<JobApplication?> GetByIdAsync(Guid id,CancellationToken ct = default);

        //fetch application AND include Job data & -> used in UpdateStatus
        //→ why? need Job.CreatedBy to verify
        //recruiter owns the job before updating status
        Task<JobApplication?> GetByIdWithJobAsync(Guid id,CancellationToken ct = default);

        //all applications by one student
        // GET /api/v1/application/get — student's applied jobs with Job + Company data
        Task<IEnumerable<JobApplication>> GetByApplicantAsync(Guid applicantId, CancellationToken ct = default);

        // GET /api/v1/application/{id}/applicants — recruiter sees applicants with resume + profile
        Task<List<JobApplication>> GetByJobAsync(Guid jobId, CancellationToken ct = default);


        Task<bool> AlreadyAppliedAsync(Guid jobId, Guid applicantId, CancellationToken ct = default);

        void Add(JobApplication application);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
