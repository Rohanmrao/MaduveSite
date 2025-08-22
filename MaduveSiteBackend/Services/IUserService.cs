using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto?> GetByIdAsync(Guid id);
    Task<UserResponseDto> CreateAsync(CreateUserDto createUserDto);
    Task<UserResponseDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto);
    Task ChangeStatusAsync(Guid id, ProfileStatus status);
}