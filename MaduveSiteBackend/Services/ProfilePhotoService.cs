using MaduveSiteBackend.Repositories;

namespace MaduveSiteBackend.Services;

public class ProfilePhotoService : IProfilePhotoService
{
    private readonly IUserRepository _userRepository;

    public ProfilePhotoService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(fileExtension))
            return false;

        var allowedMimeTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
        if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            return false;

        if (file.Length > 5 * 1024 * 1024) // 5MB limit
            return false;

        return true;
    }
}