using MaduveSiteBackend.Repositories;
using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace MaduveSiteBackend.Services;

public class LoginService : ILoginService
{
    private readonly IUserRepository _userRepository;
    private readonly IAdminRepository _adminRepository;
    private readonly IUserRequestRepository _userRequestRepository;

    public LoginService(
        IUserRepository userRepository,
        IAdminRepository adminRepository,
        IUserRequestRepository userRequestRepository)
    {
        _userRepository = userRepository;
        _adminRepository = adminRepository;
        _userRequestRepository = userRequestRepository;
    }

    public async Task<LoginResponseDto> LoginUserAsync(UserLoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        if (!VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        return new LoginResponseDto
        {
            Success = true,
            Message = "Login successful",
            UserId = user.Id,
            UserType = "User",
            UserName = user.FullName,
            Status = user.Status.ToString()
        };
    }

    public async Task<LoginResponseDto> LoginAdminAsync(AdminLoginDto loginDto)
    {
        var admin = await _adminRepository.GetByEmailAsync(loginDto.Email);
        if (admin == null)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        if (!admin.IsActive)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Account is deactivated"
            };
        }

        if (!VerifyPassword(loginDto.Password, admin.PasswordHash))
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        return new LoginResponseDto
        {
            Success = true,
            Message = "Login successful",
            UserId = admin.Id,
            UserType = "Admin",
            UserName = admin.FullName,
            Status = admin.IsActive ? "Active" : "Inactive"
        };
    }

    public async Task<ApplicationStatusDto> GetApplicationStatusAsync(string email)
    {
        // First check if user is already approved and active
        var user = await _userRepository.GetByEmailAsync(email);
        if (user != null)
        {
            return new ApplicationStatusDto
            {
                Email = email,
                Status = "Approved",
                Message = "Your account has been approved and is active",
                CreatedAt = user.CreatedAt,
                ProcessedAt = user.CreatedAt
            };
        }

        // Check if there's a pending request
        var userRequest = await _userRequestRepository.GetByEmailAsync(email);
        if (userRequest != null)
        {
            var adminName = "Unknown";
            if (userRequest.AdminId.HasValue)
            {
                var admin = await _adminRepository.GetByIdAsync(userRequest.AdminId.Value);
                if (admin != null)
                {
                    adminName = admin.FullName;
                }
            }

            var message = userRequest.Status switch
            {
                RequestStatus.Pending => "Your application is pending review",
                RequestStatus.Approved => "Your application has been approved",
                RequestStatus.Rejected => "Your application has been rejected",
                _ => "Unknown status"
            };

            return new ApplicationStatusDto
            {
                Email = email,
                Status = userRequest.Status.ToString(),
                Message = message,
                CreatedAt = userRequest.CreatedAt,
                ProcessedAt = userRequest.ProcessedAt,
                AdminName = userRequest.Status != RequestStatus.Pending ? adminName : null
            };
        }

        return new ApplicationStatusDto
        {
            Email = email,
            Status = "Not Found",
            Message = "No application found with this email address"
        };
    }

    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string password, string passwordHash)
    {
        var hashedPassword = HashPassword(password);
        return hashedPassword == passwordHash;
    }
}
