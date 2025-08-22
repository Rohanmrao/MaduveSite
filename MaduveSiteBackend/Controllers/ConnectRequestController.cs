using Microsoft.AspNetCore.Mvc;
using MaduveSiteBackend.Services;
using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConnectRequestController : ControllerBase
{
    private readonly IConnectRequestService _connectRequestService;

    public ConnectRequestController(IConnectRequestService connectRequestService)
    {
        _connectRequestService = connectRequestService;
    }

    [HttpPost("sender/{senderId}/send")]
    public async Task<IActionResult> SendConnectRequest(Guid senderId, [FromBody] SendConnectRequestDto requestDto)
    {
        var result = await _connectRequestService.SendConnectRequestAsync(senderId, requestDto);
        return Ok(result);
    }

    [HttpPost("receiver/{receiverId}/accept/{senderId}")]
    public async Task<IActionResult> AcceptConnectRequest(Guid receiverId, Guid senderId)
    {
        var result = await _connectRequestService.AcceptConnectRequestAsync(receiverId, senderId);
        return Ok(result);
    }

    [HttpPost("receiver/{receiverId}/reject/{senderId}")]
    public async Task<IActionResult> RejectConnectRequest(Guid receiverId, Guid senderId)
    {
        var result = await _connectRequestService.RejectConnectRequestAsync(receiverId, senderId);
        return Ok(result);
    }

    [HttpGet("receiver/{receiverId}/pending")]
    public async Task<IActionResult> GetPendingRequestsForReceiver(Guid receiverId)
    {
        var requests = await _connectRequestService.GetPendingRequestsForUserAsync(receiverId);
        return Ok(requests);
    }

    [HttpGet("sender/{senderId}/sent")]
    public async Task<IActionResult> GetSentRequestsBySender(Guid senderId)
    {
        var requests = await _connectRequestService.GetSentRequestsByUserAsync(senderId);
        return Ok(requests);
    }

    [HttpGet("connected/{senderId}/{receiverId}")]
    public async Task<IActionResult> CheckConnection(Guid senderId, Guid receiverId)
    {
        var hasConnection = await _connectRequestService.HasActiveConnectionAsync(senderId, receiverId);
        return Ok(new { hasConnection });
    }
}