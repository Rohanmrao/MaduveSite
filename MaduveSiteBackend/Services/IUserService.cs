using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto?> GetByIdAsync(Guid id);
    Task<UserResponseDto?> GetByEmailAsync(string email);
    Task<UserResponseDto> CreateAsync(CreateUserDto createUserDto);
    Task<UserResponseDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto);
    Task DeleteAsync(Guid id);
    Task ChangeStatusAsync(Guid id, ProfileStatus status);
    Task SendConnectRequest();
    Task DeleteConnectRequest();
    Task AcceptConnectRequest();
    Task RejectConnectRequest();
}