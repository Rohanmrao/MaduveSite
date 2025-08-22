using MaduveSiteBackend.Repositories;
using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Services;

public class ConnectRequestService : IConnectRequestService
{
    private readonly IConnectRequestRepository _connectRequestRepository;
    private readonly IUserRepository _userRepository;

    public ConnectRequestService(
        IConnectRequestRepository connectRequestRepository,
        IUserRepository userRepository)
    {
        _connectRequestRepository = connectRequestRepository;
        _userRepository = userRepository;
    }

    public async Task<ConnectRequestResponseDto> SendConnectRequestAsync(Guid senderId, SendConnectRequestDto requestDto)
    {
        var sender = await _userRepository.GetByIdAsync(senderId);
        var receiver = await _userRepository.GetByIdAsync(requestDto.ReceiverId);

        if (sender == null || receiver == null)
        {
            return new ConnectRequestResponseDto
            {
                Id = Guid.Empty,
                Status = "Error",
                Message = "Sender or receiver not found"
            };
        }

        if (sender.Status != ProfileStatus.Active || receiver.Status != ProfileStatus.Active)
        {
            return new ConnectRequestResponseDto
            {
                Id = Guid.Empty,
                Status = "Error",
                Message = "Both users must be active to send connect requests"
            };
        }

        var existingRequest = await _connectRequestRepository.GetBySenderAndReceiverAsync(senderId, requestDto.ReceiverId);
        if (existingRequest != null)
        {
            return new ConnectRequestResponseDto
            {
                Id = existingRequest.Id,
                Status = "Error",
                Message = "Connect request already exists"
            };
        }

        var hasActiveConnection = await _connectRequestRepository.HasActiveConnectionAsync(senderId, requestDto.ReceiverId);
        if (hasActiveConnection)
        {
            return new ConnectRequestResponseDto
            {
                Id = Guid.Empty,
                Status = "Error",
                Message = "Users are already connected"
            };
        }

        var connectRequest = new ConnectRequest
        {
            SenderId = senderId,
            ReceiverId = requestDto.ReceiverId,
            Message = requestDto.Message,
            Status = ConnectRequestStatus.Pending
        };

        await _connectRequestRepository.AddAsync(connectRequest);

        return new ConnectRequestResponseDto
        {
            Id = connectRequest.Id,
            Status = "Sent",
            Message = "Connect request sent successfully"
        };
    }

    public async Task<ConnectRequestResponseDto> AcceptConnectRequestAsync(Guid receiverId, Guid senderId)
    {
        var connectRequest = await _connectRequestRepository.GetBySenderAndReceiverAsync(senderId, receiverId);
        if (connectRequest == null)
        {
            return new ConnectRequestResponseDto
            {
                Id = Guid.Empty,
                Status = "Error",
                Message = "Connect request not found"
            };
        }

        if (connectRequest.ReceiverId != receiverId)
        {
            return new ConnectRequestResponseDto
            {
                Id = connectRequest.Id,
                Status = "Error",
                Message = "You can only accept requests sent to you"
            };
        }

        if (connectRequest.Status != ConnectRequestStatus.Pending)
        {
            return new ConnectRequestResponseDto
            {
                Id = connectRequest.Id,
                Status = "Error",
                Message = "Connect request is not pending"
            };
        }

        connectRequest.Status = ConnectRequestStatus.Accepted;
        connectRequest.RespondedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        connectRequest.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        await _connectRequestRepository.UpdateAsync(connectRequest);

        var sender = await _userRepository.GetByIdAsync(connectRequest.SenderId);
        var receiver = await _userRepository.GetByIdAsync(connectRequest.ReceiverId);

        if (sender != null && receiver != null)
        {
            sender.Status = ProfileStatus.InTalks;
            sender.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            receiver.Status = ProfileStatus.InTalks;
            receiver.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            await _userRepository.UpdateAsync(sender);
            await _userRepository.UpdateAsync(receiver);
        }

        return new ConnectRequestResponseDto
        {
            Id = connectRequest.Id,
            Status = "Accepted",
            Message = "Connect request accepted successfully"
        };
    }

    public async Task<ConnectRequestResponseDto> RejectConnectRequestAsync(Guid receiverId, Guid senderId)
    {
        var connectRequest = await _connectRequestRepository.GetBySenderAndReceiverAsync(senderId, receiverId);
        if (connectRequest == null)
        {
            return new ConnectRequestResponseDto
            {
                Id = Guid.Empty,
                Status = "Error",
                Message = "Connect request not found"
            };
        }

        if (connectRequest.ReceiverId != receiverId)
        {
            return new ConnectRequestResponseDto
            {
                Id = connectRequest.Id,
                Status = "Error",
                Message = "You can only reject requests sent to you"
            };
        }

        if (connectRequest.Status != ConnectRequestStatus.Pending)
        {
            return new ConnectRequestResponseDto
            {
                Id = connectRequest.Id,
                Status = "Error",
                Message = "Connect request is not pending"
            };
        }

        connectRequest.Status = ConnectRequestStatus.Rejected;
        connectRequest.RespondedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        connectRequest.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        await _connectRequestRepository.UpdateAsync(connectRequest);

        return new ConnectRequestResponseDto
        {
            Id = connectRequest.Id,
            Status = "Rejected",
            Message = "Connect request rejected successfully"
        };
    }

    public async Task<IEnumerable<ConnectRequestDto>> GetPendingRequestsForUserAsync(Guid receiverId)
    {
        var pendingRequests = await _connectRequestRepository.GetPendingRequestsForUserAsync(receiverId);
        return pendingRequests.Select(r => new ConnectRequestDto
        {
            Id = r.Id,
            SenderId = r.SenderId,
            ReceiverId = r.ReceiverId,
            SenderName = r.Sender.FullName,
            ReceiverName = r.Receiver.FullName,
            Message = r.Message,
            Status = r.Status.ToString(),
            CreatedAt = r.CreatedAt
        });
    }

    public async Task<IEnumerable<ConnectRequestDto>> GetSentRequestsByUserAsync(Guid senderId)
    {
        var sentRequests = await _connectRequestRepository.GetSentRequestsByUserAsync(senderId);
        return sentRequests.Select(r => new ConnectRequestDto
        {
            Id = r.Id,
            SenderId = r.SenderId,
            ReceiverId = r.ReceiverId,
            SenderName = r.Sender.FullName,
            ReceiverName = r.Receiver.FullName,
            Message = r.Message,
            Status = r.Status.ToString(),
            CreatedAt = r.CreatedAt
        });
    }

    public async Task<bool> HasActiveConnectionAsync(Guid senderId, Guid receiverId)
    {
        return await _connectRequestRepository.HasActiveConnectionAsync(senderId, receiverId);
    }
}
