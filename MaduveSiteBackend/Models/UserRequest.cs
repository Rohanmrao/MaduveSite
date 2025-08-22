using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaduveSiteBackend.Models;

[Table("user_requests")]
public class UserRequest
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [Column("full_name")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;
    
    [Required]
    [Column("email")]
    [StringLength(255)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [Column("password_hash")]
    [StringLength(255)]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Column("phone")]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;
    
    [Column("ecclesia")]
    [StringLength(100)]
    public string Ecclesia { get; set; } = string.Empty;
    
    [Column("language")]
    [StringLength(50)]
    public string Language { get; set; } = string.Empty;
    
    [Column("education")]
    [StringLength(100)]
    public string Education { get; set; } = string.Empty;
    
    [Column("bio")]
    [StringLength(1000)]
    public string Bio { get; set; } = string.Empty;
    
    [Column("profile_photo_data")]
    public byte[]? ProfilePhotoData { get; set; }
    
    [Column("profile_photo_content_type")]
    [StringLength(100)]
    public string? ProfilePhotoContentType { get; set; }
    
    [Column("status")]
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    
    [Column("admin_id")]
    public Guid? AdminId { get; set; }
    
    [Column("processed_at")]
    public DateTime? ProcessedAt { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

public enum RequestStatus
{
    Pending,
    Approved,
    Rejected
}
