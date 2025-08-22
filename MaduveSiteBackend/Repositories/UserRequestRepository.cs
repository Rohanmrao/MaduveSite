using Microsoft.EntityFrameworkCore;
using MaduveSiteBackend.Data;
using MaduveSiteBackend.Models;

namespace MaduveSiteBackend.Repositories;

public class UserRequestRepository : GenericRepository<UserRequest>, IUserRequestRepository
{
    public UserRequestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<UserRequest>> GetPendingRequestsAsync()
    {
        return await _context.UserRequests
            .Where(r => r.Status == RequestStatus.Pending)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<UserRequest?> GetByEmailAsync(string email)
    {
        return await _context.UserRequests.FirstOrDefaultAsync(r => r.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.UserRequests.AnyAsync(r => r.Email == email);
    }
}
