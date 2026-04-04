using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Job_portal.Infrastructure.Persistence.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Companies
                .Include(C => C.Creator)
                .ToListAsync(ct);
        }

        public async Task<Company?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.Id == id,ct);
        }

        public async Task<Company?> GetByIdForRecruiterAsync(Guid id, Guid recruiterId, CancellationToken ct = default)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(C => C.CreatedBy == recruiterId && C.Id == id, ct);
                
        }

        //recruiter sees ONLY their own companies
        public async Task<IEnumerable<Company>> GetByRecruiterAsync(Guid recruiterId, CancellationToken ct = default)
        {
            return await _context.Companies
                .Where(C => C.CreatedBy == recruiterId && !C.IsRemoved) //!c.IsRemoved filters out soft deleted Companies
                .ToListAsync(ct);
        }
        public async Task<bool> ExistsByNameForRecruiterAsync(string name, Guid recruiterId, CancellationToken ct = default)
        {
            return await _context.Companies
                .AnyAsync(C => C.Name.ToLower() == name.ToLower() && C.CreatedBy == recruiterId, ct);
            //toLower because  "Google" == "google" == "GOOGLE" → blocked ✅

        }

        public void Add(Company company)
        {
            _context.Companies.Add(company);
        }

        public void Remove(Company company)
        {
            _context.Companies.Remove(company);
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}
