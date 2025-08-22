using Microsoft.EntityFrameworkCore;
using MaduveSiteBackend.Data;
using MaduveSiteBackend.Models;

namespace MaduveSiteBackend.Repositories;

public class ConnectRequestRepository : GenericRepository<ConnectRequest>, IConnectRequestRepository
{
    public ConnectRequestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ConnectRequest>> GetPendingRequestsForUserAsync(Guid receiverId)
    {
        return await _context.ConnectRequests
            .Include(cr => cr.Sender)
            .Include(cr => cr.Receiver)
            .Where(cr => cr.ReceiverId == receiverId && cr.Status == ConnectRequestStatus.Pending)
            .OrderByDescending(cr => cr.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<ConnectRequest>> GetSentRequestsByUserAsync(Guid senderId)
    {
        return await _context.ConnectRequests
            .Include(cr => cr.Sender)
            .Include(cr => cr.Receiver)
            .Where(cr => cr.SenderId == senderId)
            .OrderByDescending(cr => cr.CreatedAt)
            .ToListAsync();
    }

    public async Task<ConnectRequest?> GetBySenderAndReceiverAsync(Guid senderId, Guid receiverId)
    {
        return await _context.ConnectRequests
            .Include(cr => cr.Sender)
            .Include(cr => cr.Receiver)
            .FirstOrDefaultAsync(cr => cr.SenderId == senderId && cr.ReceiverId == receiverId);
    }

    public async Task<bool> HasActiveConnectionAsync(Guid senderId, Guid receiverId)
    {
        return await _context.ConnectRequests
            .AnyAsync(cr => 
                ((cr.SenderId == senderId && cr.ReceiverId == receiverId) ||
                 (cr.SenderId == receiverId && cr.ReceiverId == senderId)) &&
                cr.Status == ConnectRequestStatus.Accepted);
    }
}
