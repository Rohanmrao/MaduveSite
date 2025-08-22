using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaduveSiteBackend.Models;

[Table("connect_requests")]
public class ConnectRequest
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [Column("sender_id")]
    public Guid SenderId { get; set; }
    
    [Required]
    [Column("receiver_id")]
    public Guid ReceiverId { get; set; }
    
    [Column("message")]
    [StringLength(500)]
    public string Message { get; set; } = string.Empty;
    
    [Column("status")]
    public ConnectRequestStatus Status { get; set; } = ConnectRequestStatus.Pending;
    
    [Column("responded_at")]
    public DateTime? RespondedAt { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    [ForeignKey("SenderId")]
    public virtual User Sender { get; set; } = null!;
    
    [ForeignKey("ReceiverId")]
    public virtual User Receiver { get; set; } = null!;
}

public enum ConnectRequestStatus
{
    Pending,
    Accepted,
    Rejected
}
