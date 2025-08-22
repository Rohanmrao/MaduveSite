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
    
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
    
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

public class UserLoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public string? UserType { get; set; }
    public string? UserName { get; set; }
    public string? Status { get; set; }
}

public class ApplicationStatusDto
{
    public string Email { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? AdminName { get; set; }
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
