using AutoMapper;
using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Services;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // CreateUserDto -> User
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProfilePhotoData, opt => opt.Ignore())
            .ForMember(dest => dest.ProfilePhotoContentType, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ProfileStatus.Pending))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        // UpdateUserDto -> User
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProfilePhotoData, opt => opt.Ignore())
            .ForMember(dest => dest.ProfilePhotoContentType, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        // User -> UserResponseDto
        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.HasProfilePhoto, opt => opt.MapFrom(src => src.ProfilePhotoData != null && src.ProfilePhotoData.Length > 0));
    }
}
