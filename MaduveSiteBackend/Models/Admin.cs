using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaduveSiteBackend.Models;

[Table("admins")]
public class Admin
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
    
    [Column("is_active")]
    public bool IsActive { get; set; } = true;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
