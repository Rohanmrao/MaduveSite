using Microsoft.EntityFrameworkCore;
using MaduveSiteBackend.Data;
using MaduveSiteBackend.Models;

namespace MaduveSiteBackend.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetByStatusAsync(ProfileStatus status)
    {
        return await _context.Users.Where(u => u.Status == status).ToListAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}