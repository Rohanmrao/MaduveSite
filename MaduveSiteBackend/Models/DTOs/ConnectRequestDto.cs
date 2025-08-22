namespace MaduveSiteBackend.Models.DTOs;

public class ConnectRequestDto
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class SendConnectRequestDto
{
    public Guid ReceiverId { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ConnectRequestResponseDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
