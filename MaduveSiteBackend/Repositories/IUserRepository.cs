using MaduveSiteBackend.Models;

namespace MaduveSiteBackend.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetByStatusAsync(ProfileStatus status);
    Task<bool> EmailExistsAsync(string email);
}