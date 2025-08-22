using MaduveSiteBackend.Repositories;
using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Services;

public class UserRequestService : IUserRequestService
{
    private readonly IUserRequestRepository _userRequestRepository;
    private readonly IUserRepository _userRepository;

    public UserRequestService(IUserRequestRepository userRequestRepository, IUserRepository userRepository)
    {
        _userRequestRepository = userRequestRepository;
        _userRepository = userRepository;
    }

    public async Task<UserRequestDto> CreateUserRequestAsync(CreateUserDto createUserDto)
    {
        if (await _userRequestRepository.EmailExistsAsync(createUserDto.Email))
        {
            throw new InvalidOperationException("A request with this email already exists");
        }

        if (await _userRepository.EmailExistsAsync(createUserDto.Email))
        {
            throw new InvalidOperationException("An account with this email already exists");
        }

        var userRequest = new UserRequest
        {
            FullName = createUserDto.FullName,
            Email = createUserDto.Email,
            PasswordHash = LoginService.HashPassword(createUserDto.Password),
            Phone = createUserDto.Phone,
            Ecclesia = createUserDto.Ecclesia,
            Language = createUserDto.Language,
            Education = createUserDto.Education,
            Bio = createUserDto.Bio,
            Status = RequestStatus.Pending
        };

        await _userRequestRepository.AddAsync(userRequest);

        return new UserRequestDto
        {
            Id = userRequest.Id,
            FullName = userRequest.FullName,
            Email = userRequest.Email,
            Phone = userRequest.Phone,
            Ecclesia = userRequest.Ecclesia,
            Language = userRequest.Language,
            Education = userRequest.Education,
            Bio = userRequest.Bio,
            Status = userRequest.Status.ToString(),
            CreatedAt = userRequest.CreatedAt
        };
    }

    public async Task<UserRequestDto?> GetUserRequestByEmailAsync(string email)
    {
        var userRequest = await _userRequestRepository.GetByEmailAsync(email);
        if (userRequest == null)
            return null;

        return new UserRequestDto
        {
            Id = userRequest.Id,
            FullName = userRequest.FullName,
            Email = userRequest.Email,
            Phone = userRequest.Phone,
            Ecclesia = userRequest.Ecclesia,
            Language = userRequest.Language,
            Education = userRequest.Education,
            Bio = userRequest.Bio,
            Status = userRequest.Status.ToString(),
            CreatedAt = userRequest.CreatedAt
        };
    }

    public async Task<bool> EmailExistsInRequestsAsync(string email)
    {
        return await _userRequestRepository.EmailExistsAsync(email);
    }
}
