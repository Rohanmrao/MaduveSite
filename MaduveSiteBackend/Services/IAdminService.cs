using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Services;

public interface IAdminService
{
    Task<IEnumerable<string>> GetDashboardStatsAsync();
    Task<UserRequestResponseDto> ApproveUserAsync(Guid userId, Guid adminId);
    Task<UserRequestResponseDto> RejectUserAsync(Guid userId, Guid adminId);
    Task<IEnumerable<UserRequestDto>> GetPendingRequestsAsync();
    Task DeletePendingRequestsAsync();
    Task<UserRequestDto?> GetPendingRequestByIdAsync(Guid id);
    Task DeletePendingRequestByIdAsync(Guid id);
    Task<AdminResponseDto> AddAdminAsync(AdminSignupDto adminDto);
    Task RemoveAdminAsync(Guid adminId);
    Task<AdminResponseDto> UpdateAdminAsync(Guid adminId, AdminUpdateDto adminDto);
    Task<AdminResponseDto?> GetAdminByIdAsync(Guid adminId);
    Task<IEnumerable<AdminResponseDto>> GetAllAdminsAsync();
    Task<bool> IsAdminAsync(Guid adminId);
    Task<string> GetAuthLevelAsync(Guid userId);
    Task<AdminResponseDto?> GetAdminDetailsByEmailAsync(string email);
}