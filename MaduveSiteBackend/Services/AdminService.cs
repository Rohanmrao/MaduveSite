using MaduveSiteBackend.Repositories;
using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace MaduveSiteBackend.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IAdminRepository _adminRepository;
    private readonly IUserRequestRepository _userRequestRepository;

    public AdminService(
        IUserRepository userRepository,
        IAdminRepository adminRepository,
        IUserRequestRepository userRequestRepository)
    {
        _userRepository = userRepository;
        _adminRepository = adminRepository;
        _userRequestRepository = userRequestRepository;
    }

    public async Task<IEnumerable<string>> GetDashboardStatsAsync()
    {
        var users = (await _userRepository.GetAllAsync()).ToList();
        var userRequests = (await _userRequestRepository.GetAllAsync()).ToList();
        var admins = (await _adminRepository.GetAllAsync()).ToList();
        
        var activeUsers = users.Count(u => u.Status == ProfileStatus.Active);
        var pendingUsers = userRequests.Count(r => r.Status == RequestStatus.Pending);
        var activeAdmins = admins.Count(a => a.IsActive);

        return new List<string>
        {
            $"Total Users: {users.Count}",
            $"Active Users: {activeUsers}",
            $"Pending Approvals: {pendingUsers}",
            $"Total Admins: {admins.Count}",
            $"Active Admins: {activeAdmins}"
        };
    }

    public async Task<UserRequestResponseDto> ApproveUserAsync(Guid userId, Guid adminId)
    {
        var userRequest = await _userRequestRepository.GetByIdAsync(userId);
        if (userRequest == null)
        {
            return new UserRequestResponseDto
            {
                Id = userId,
                Status = "Error",
                Message = "User request not found"
            };
        }

        if (userRequest.Status != RequestStatus.Pending)
        {
            return new UserRequestResponseDto
            {
                Id = userId,
                Status = "Error",
                Message = "User request is not pending"
            };
        }

        var user = new User
        {
            FullName = userRequest.FullName,
            Email = userRequest.Email,
            PasswordHash = userRequest.PasswordHash,
            Phone = userRequest.Phone,
            Ecclesia = userRequest.Ecclesia,
            Language = userRequest.Language,
            Education = userRequest.Education,
            Bio = userRequest.Bio,
            ProfilePhotoData = userRequest.ProfilePhotoData,
            ProfilePhotoContentType = userRequest.ProfilePhotoContentType,
            ProfileImage1Data = userRequest.ProfileImage1Data,
            ProfileImage1ContentType = userRequest.ProfileImage1ContentType,
            ProfileImage2Data = userRequest.ProfileImage2Data,
            ProfileImage2ContentType = userRequest.ProfileImage2ContentType,
            ProfileImage3Data = userRequest.ProfileImage3Data,
            ProfileImage3ContentType = userRequest.ProfileImage3ContentType,
            Status = ProfileStatus.Active
        };

        await _userRepository.AddAsync(user);

        userRequest.Status = RequestStatus.Approved;
        userRequest.AdminId = adminId;
        userRequest.ProcessedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        userRequest.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        await _userRequestRepository.UpdateAsync(userRequest);

        return new UserRequestResponseDto
        {
            Id = userId,
            Status = "Approved",
            Message = "User approved successfully"
        };
    }

    public async Task<UserRequestResponseDto> RejectUserAsync(Guid userId, Guid adminId)
    {
        var userRequest = await _userRequestRepository.GetByIdAsync(userId);
        if (userRequest == null)
        {
            return new UserRequestResponseDto
            {
                Id = userId,
                Status = "Error",
                Message = "User request not found"
            };
        }

        if (userRequest.Status != RequestStatus.Pending)
        {
            return new UserRequestResponseDto
            {
                Id = userId,
                Status = "Error",
                Message = "User request is not pending"
            };
        }

        userRequest.Status = RequestStatus.Rejected;
        userRequest.AdminId = adminId;
        userRequest.ProcessedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        userRequest.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        await _userRequestRepository.UpdateAsync(userRequest);

        return new UserRequestResponseDto
        {
            Id = userId,
            Status = "Rejected",
            Message = "User rejected successfully"
        };
    }

    public async Task<IEnumerable<UserRequestDto>> GetPendingRequestsAsync()
    {
        var pendingRequests = await _userRequestRepository.GetPendingRequestsAsync();
        return pendingRequests.Select(r => new UserRequestDto
        {
            Id = r.Id,
            FullName = r.FullName,
            Email = r.Email,
            Phone = r.Phone,
            Ecclesia = r.Ecclesia,
            Language = r.Language,
            Education = r.Education,
            Bio = r.Bio,
            Status = r.Status.ToString(),
            CreatedAt = r.CreatedAt
        });
    }

    public async Task DeletePendingRequestsAsync()
    {
        var pendingRequests = await _userRequestRepository.GetPendingRequestsAsync();
        foreach (var request in pendingRequests)
        {
            await _userRequestRepository.DeleteAsync(request.Id);
        }
    }

    public async Task<UserRequestDto?> GetPendingRequestByIdAsync(Guid id)
    {
        var request = await _userRequestRepository.GetByIdAsync(id);
        if (request == null || request.Status != RequestStatus.Pending)
            return null;

        return new UserRequestDto
        {
            Id = request.Id,
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            Ecclesia = request.Ecclesia,
            Language = request.Language,
            Education = request.Education,
            Bio = request.Bio,
            Status = request.Status.ToString(),
            CreatedAt = request.CreatedAt
        };
    }

    public async Task DeletePendingRequestByIdAsync(Guid id)
    {
        await _userRequestRepository.DeleteAsync(id);
    }

    public async Task<AdminResponseDto> AddAdminAsync(AdminSignupDto adminDto)
    {
        if (await _adminRepository.EmailExistsAsync(adminDto.Email))
        {
            throw new InvalidOperationException("Admin with this email already exists");
        }

        var passwordHash = HashPassword(adminDto.Password);
        var admin = new Admin
        {
            FullName = adminDto.FullName,
            Email = adminDto.Email,
            PasswordHash = passwordHash,
            Phone = adminDto.Phone,
            IsActive = true
        };

        await _adminRepository.AddAsync(admin);

        return new AdminResponseDto
        {
            Id = admin.Id,
            FullName = admin.FullName,
            Email = admin.Email,
            Phone = admin.Phone,
            IsActive = admin.IsActive,
            CreatedAt = admin.CreatedAt
        };
    }

    public async Task RemoveAdminAsync(Guid adminId)
    {
        var admin = await _adminRepository.GetByIdAsync(adminId);
        if (admin != null)
        {
            admin.IsActive = false;
            admin.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            await _adminRepository.UpdateAsync(admin);
        }
    }

    public async Task<AdminResponseDto> UpdateAdminAsync(Guid adminId, AdminUpdateDto adminDto)
    {
        var admin = await _adminRepository.GetByIdAsync(adminId);
        if (admin == null)
        {
            throw new InvalidOperationException("Admin not found");
        }

        admin.FullName = adminDto.FullName;
        admin.Phone = adminDto.Phone;
        admin.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

        await _adminRepository.UpdateAsync(admin);

        return new AdminResponseDto
        {
            Id = admin.Id,
            FullName = admin.FullName,
            Email = admin.Email,
            Phone = admin.Phone,
            IsActive = admin.IsActive,
            CreatedAt = admin.CreatedAt
        };
    }

    public async Task<AdminResponseDto?> GetAdminByIdAsync(Guid adminId)
    {
        var admin = await _adminRepository.GetByIdAsync(adminId);
        if (admin == null)
            return null;

        return new AdminResponseDto
        {
            Id = admin.Id,
            FullName = admin.FullName,
            Email = admin.Email,
            Phone = admin.Phone,
            IsActive = admin.IsActive,
            CreatedAt = admin.CreatedAt
        };
    }

    public async Task<IEnumerable<AdminResponseDto>> GetAllAdminsAsync()
    {
        var admins = await _adminRepository.GetAllAsync();
        return admins.Select(a => new AdminResponseDto
        {
            Id = a.Id,
            FullName = a.FullName,
            Email = a.Email,
            Phone = a.Phone,
            IsActive = a.IsActive,
            CreatedAt = a.CreatedAt
        });
    }

    public async Task<bool> IsAdminAsync(Guid adminId)
    {
        var admin = await _adminRepository.GetByIdAsync(adminId);
        return admin != null && admin.IsActive;
    }

    public async Task<string> GetAuthLevelAsync(Guid userId)
    {
        var admin = await _adminRepository.GetByIdAsync(userId);
        if (admin != null && admin.IsActive)
        {
            return "Admin";
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user != null)
        {
            return "User";
        }

        return "Unknown";
    }

    public async Task<AdminResponseDto?> GetAdminDetailsByEmailAsync(string email)
    {
        var admin = await _adminRepository.GetByEmailAsync(email);
        if (admin == null)
            return null;

        return new AdminResponseDto
        {
            Id = admin.Id,
            FullName = admin.FullName,
            Email = admin.Email,
            Phone = admin.Phone,
            IsActive = admin.IsActive,
            CreatedAt = admin.CreatedAt
        };
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}