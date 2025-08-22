using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.DTOs;
using MaduveSiteBackend.Repositories;

namespace MaduveSiteBackend.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;

    public UserService(IUserRepository userRepository, IUserMapper userMapper)
    {
        _userRepository = userRepository;
        _userMapper = userMapper;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _userMapper.MapToResponseDtos(users);
    }

    public async Task<UserResponseDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user != null ? _userMapper.MapToResponseDto(user) : null;
    }

    public async Task<UserResponseDto?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user != null ? _userMapper.MapToResponseDto(user) : null;
    }

    public async Task<UserResponseDto> CreateAsync(CreateUserDto createUserDto)
    {
        var user = _userMapper.MapToEntity(createUserDto);
        var createdUser = await _userRepository.CreateAsync(user);
        return _userMapper.MapToResponseDto(createdUser);
    }

    public async Task<UserResponseDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
            throw new ArgumentException("User not found");

        var updatedUser = _userMapper.MapToEntity(updateUserDto, existingUser);
        var savedUser = await _userRepository.UpdateAsync(updatedUser);
        return _userMapper.MapToResponseDto(savedUser);
    }

    public async Task DeleteAsync(Guid id)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
            throw new ArgumentException("User not found");

        await _userRepository.DeleteAsync(id);
    }

    public async Task ChangeStatusAsync(Guid id, ProfileStatus status)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser != null)
        {
            existingUser.Status = status;
            existingUser.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            await _userRepository.UpdateAsync(existingUser);
        }
    }

    public Task SendConnectRequest()
    {
        throw new NotImplementedException();
    }

    public Task DeleteConnectRequest()
    {
        throw new NotImplementedException();
    }

    public Task AcceptConnectRequest()
    {
        throw new NotImplementedException();
    }

    public Task RejectConnectRequest()
    {
        throw new NotImplementedException();
    }
}