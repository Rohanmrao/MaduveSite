using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Services;

public interface IConnectRequestService
{
    Task<ConnectRequestResponseDto> SendConnectRequestAsync(Guid senderId, SendConnectRequestDto requestDto);
    Task<ConnectRequestResponseDto> AcceptConnectRequestAsync(Guid receiverId, Guid senderId);
    Task<ConnectRequestResponseDto> RejectConnectRequestAsync(Guid receiverId, Guid senderId);
    Task<IEnumerable<ConnectRequestDto>> GetPendingRequestsForUserAsync(Guid receiverId);
    Task<IEnumerable<ConnectRequestDto>> GetSentRequestsByUserAsync(Guid senderId);
    Task<bool> HasActiveConnectionAsync(Guid senderId, Guid receiverId);
}
