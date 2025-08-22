using AutoMapper;
using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Services;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => LoginService.HashPassword(src.Password)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ProfileStatus.Pending))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)));

        CreateMap<UserRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ProfileStatus.Active))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)));

        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.HasProfilePhoto, opt => opt.MapFrom(src => src.ProfilePhotoData != null && src.ProfilePhotoData.Length > 0))
            .ForMember(dest => dest.HasProfileImage1, opt => opt.MapFrom(src => src.ProfileImage1Data != null && src.ProfileImage1Data.Length > 0))
            .ForMember(dest => dest.HasProfileImage2, opt => opt.MapFrom(src => src.ProfileImage2Data != null && src.ProfileImage2Data.Length > 0))
            .ForMember(dest => dest.HasProfileImage3, opt => opt.MapFrom(src => src.ProfileImage3Data != null && src.ProfileImage3Data.Length > 0));

        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)));

        CreateMap<Admin, AdminResponseDto>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive.ToString()));

        CreateMap<UserRequest, UserRequestResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
