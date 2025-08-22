using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Services;

public interface IUserRequestService
{
    Task<UserRequestDto> CreateUserRequestAsync(CreateUserDto createUserDto);
    Task<UserRequestDto?> GetUserRequestByEmailAsync(string email);
    Task<bool> EmailExistsInRequestsAsync(string email);
}
