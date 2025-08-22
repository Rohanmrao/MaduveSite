using MaduveSiteBackend.Models;

namespace MaduveSiteBackend.Repositories;

public interface IConnectRequestRepository : IGenericRepository<ConnectRequest>
{
    Task<IEnumerable<ConnectRequest>> GetPendingRequestsForUserAsync(Guid receiverId);
    Task<IEnumerable<ConnectRequest>> GetSentRequestsByUserAsync(Guid senderId);
    Task<ConnectRequest?> GetBySenderAndReceiverAsync(Guid senderId, Guid receiverId);
    Task<bool> HasActiveConnectionAsync(Guid senderId, Guid receiverId);
}
