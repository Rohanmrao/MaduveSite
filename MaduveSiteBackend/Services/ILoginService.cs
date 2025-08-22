using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Services;

public interface ILoginService
{
    Task<LoginResponseDto> LoginUserAsync(UserLoginDto loginDto);
    Task<LoginResponseDto> LoginAdminAsync(AdminLoginDto loginDto);
    Task<ApplicationStatusDto> GetApplicationStatusAsync(string email);
}
