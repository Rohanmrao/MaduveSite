using System.ComponentModel.DataAnnotations;

namespace MaduveSiteBackend.Models.DTOs;

public class CreateUserDto
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string Ecclesia { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string Language { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string Education { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string Bio { get; set; } = string.Empty;
}

public class UpdateUserDto
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string Ecclesia { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string Language { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string Education { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string Bio { get; set; } = string.Empty;
}

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Ecclesia { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Education { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public bool HasProfilePhoto { get; set; }
    public ProfileStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ChangeStatusDto
{
    public ProfileStatus Status { get; set; }
}
