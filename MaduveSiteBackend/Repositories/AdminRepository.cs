using Microsoft.EntityFrameworkCore;
using MaduveSiteBackend.Data;
using MaduveSiteBackend.Models;

namespace MaduveSiteBackend.Repositories;

public class AdminRepository : GenericRepository<Admin>, IAdminRepository
{
    public AdminRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Admin?> GetByEmailAsync(string email)
    {
        return await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Admins.AnyAsync(a => a.Email == email);
    }
}
