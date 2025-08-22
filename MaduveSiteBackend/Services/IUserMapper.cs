using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Services;

public interface IUserMapper
{
    User MapToEntity(CreateUserDto dto);
    User MapToEntity(UpdateUserDto dto, User existingUser);
    UserResponseDto MapToResponseDto(User user);
    IEnumerable<UserResponseDto> MapToResponseDtos(IEnumerable<User> users);
}
