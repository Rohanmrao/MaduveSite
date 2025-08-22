using MaduveSiteBackend.Models;

namespace MaduveSiteBackend.Repositories;

public interface IUserRequestRepository : IGenericRepository<UserRequest>
{
    Task<IEnumerable<UserRequest>> GetPendingRequestsAsync();
    Task<UserRequest?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
}
