using Job_portal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Job_portal.Application.Common.Interfaces.Repositories
{
    // GetByIdAsync            → no ownership check(anyone can view)
    //GetByIdForRecruiterAsync → ownership check(only owner can edit/delete)
    //GetAllAsync              → no filter(admin sees all)
    //GetByRecruiterAsync      → filtered(recruiter sees own only)
    public interface ICompanyRepository
    {
        Task<Company?> GetByIdAsync(Guid id,CancellationToken ct = default);
        Task<Company?> GetByIdForRecruiterAsync(Guid id,Guid recruiterId, CancellationToken ct = default);


        Task<IEnumerable<Company>> GetAllAsync(CancellationToken ct = default);
        Task<IEnumerable<Company>> GetByRecruiterAsync(Guid recruiterId, CancellationToken ct = default);


        //One recruiter CANNOT create TWO companies with SAME NAME
        Task<bool> ExistsByNameForRecruiterAsync(string name,Guid recruiterId, CancellationToken ct = default);


        void Add(Company company);
        void Remove(Company company);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
