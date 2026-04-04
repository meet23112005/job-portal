using Job_portal.Domain.Entities;

namespace Job_portal.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        //used in → UpdateProfile, RemoveUser
        Task<User?> GetByIdAsync(Guid id,CancellationToken ct = default);

        //used in → Login (email + password check)

        Task<User?> GetByEmailAsync(string email,CancellationToken ct = default);
        //UpdateProfile (needs profile to update bio/resume)
        Task<User?> GetByIdWithProfileAsync(Guid id, CancellationToken ct = default);



        Task<IEnumerable<User?>> GetAllAsync(CancellationToken ct=default);
        Task<IEnumerable<User?>> GetAllRecruitersAsync(CancellationToken ct = default);
        Task<IEnumerable<User?>> GetAllJobSeekersAsync(CancellationToken ct = default);

        //Register (prevent duplicate email)
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
        Task<User?> GetByEmailWithProfileAsync(string email, CancellationToken ct = default);


        void Add(User user);
        void Remove(User user);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
