namespace MaduveSiteBackend.Models.DTOs;

public class UserRequestDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Ecclesia { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Education { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class UserRequestResponseDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
