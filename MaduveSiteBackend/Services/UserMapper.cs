using AutoMapper;
using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Services;

public class UserMapper : IUserMapper
{
    private readonly IMapper _mapper;

    public UserMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public User MapToEntity(CreateUserDto dto)
    {
        return _mapper.Map<User>(dto);
    }

    public User MapToEntity(UpdateUserDto dto, User existingUser)
    {
        _mapper.Map(dto, existingUser);
        return existingUser;
    }

    public UserResponseDto MapToResponseDto(User user)
    {
        return _mapper.Map<UserResponseDto>(user);
    }

    public IEnumerable<UserResponseDto> MapToResponseDtos(IEnumerable<User> users)
    {
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }
}
