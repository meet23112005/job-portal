using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Domain.Entities;
using Job_portal.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Job_portal.Infrastructure.Persistence.Repositories;

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email, ct);
        }
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        //used in UpdateProfile so include userProfile is required.
        public async Task<User?> GetByIdWithProfileAsync(Guid id, CancellationToken ct = default)
        {
            var user = await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == id, ct);
            return user;
        }
        public async Task<IEnumerable<User?>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<User?>> GetAllJobSeekersAsync(CancellationToken ct = default)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .Where(u => u.Role == UserRole.Student)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<User?>> GetAllRecruitersAsync(CancellationToken ct = default)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .Where(u => u.Role == UserRole.Recruiter)
                .ToListAsync(ct);
        }
        public async Task<User?> GetByEmailWithProfileAsync(string email, CancellationToken ct = default)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Email == email, ct);
        }
        public async Task<User?> GetByConfirmationTokenAsync(string token, CancellationToken ct = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.EmailConfirmationToken == token, ct);
        }
        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
        {
            return await _context.Users.AnyAsync(u => u.Email == email, ct);
        }
        public async Task<User?> GetByResetTokenAsync(string token, CancellationToken ct = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => 
                                    u.PasswordResetToken == token && 
                                    u.ResetTokenExpiresAt > DateTime.UtcNow, ct);
        }



        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public void Remove(User user)
        {
            _context.Users.Remove(user);
        }



        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

    }  

