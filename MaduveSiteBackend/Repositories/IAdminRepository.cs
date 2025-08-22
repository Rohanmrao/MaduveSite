using MaduveSiteBackend.Models;

namespace MaduveSiteBackend.Repositories;

public interface IAdminRepository : IGenericRepository<Admin>
{
    Task<Admin?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
}
