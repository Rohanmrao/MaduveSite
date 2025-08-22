using MaduveSiteBackend.Repositories;
using MaduveSiteBackend.Configuration;

namespace MaduveSiteBackend.Services;

public class ProfilePhotoService : IProfilePhotoService
{
    private readonly IUserRepository _userRepository;
    private readonly AppSettings _appSettings;

    public ProfilePhotoService(IUserRepository userRepository, AppSettings appSettings)
    {
        _userRepository = userRepository;
        _appSettings = appSettings;
    }

    public async Task<byte[]> UploadPhotoAsync(Guid userId, IFormFile photoFile)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("User not found");

        if (!IsValidImageFile(photoFile))
            throw new ArgumentException("Invalid image file");

        using var memoryStream = new MemoryStream();
        await photoFile.CopyToAsync(memoryStream);
        var photoData = memoryStream.ToArray();

        user.ProfilePhotoData = photoData;
        user.ProfilePhotoContentType = photoFile.ContentType;
        user.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        
        await _userRepository.UpdateAsync(user);
        return photoData;
    }

    public async Task<bool> DeletePhotoAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        user.ProfilePhotoData = null;
        user.ProfilePhotoContentType = null;
        user.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<(byte[]? data, string? contentType)> GetPhotoAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user != null ? (user.ProfilePhotoData, user.ProfilePhotoContentType) : (null, null);
    }

    private bool IsValidImageFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_appSettings.AllowedImageExtensions.Contains(fileExtension))
            return false;

        if (!_appSettings.AllowedImageMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            return false;

        if (file.Length > _appSettings.MaxImageSizeBytes)
            return false;

        return true;
    }
}