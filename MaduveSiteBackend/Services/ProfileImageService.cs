using MaduveSiteBackend.Repositories;
using MaduveSiteBackend.Configuration;

namespace MaduveSiteBackend.Services;

public class ProfileImageService : IProfileImageService
{
    private readonly IUserRepository _userRepository;
    private readonly AppSettings _appSettings;

    public ProfileImageService(IUserRepository userRepository, AppSettings appSettings)
    {
        _userRepository = userRepository;
        _appSettings = appSettings;
    }

    public async Task<byte[]> UploadProfileImageAsync(Guid userId, int imageNumber, IFormFile imageFile)
    {
        if (imageNumber < 1 || imageNumber > _appSettings.MaxProfileImagesPerUser)
            throw new ArgumentException($"Image number must be between 1 and {_appSettings.MaxProfileImagesPerUser}");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new ArgumentException("User not found");

        if (!IsValidImageFile(imageFile))
            throw new ArgumentException("Invalid image file");

        using var memoryStream = new MemoryStream();
        await imageFile.CopyToAsync(memoryStream);
        var imageData = memoryStream.ToArray();

        switch (imageNumber)
        {
            case 1:
                user.ProfileImage1Data = imageData;
                user.ProfileImage1ContentType = imageFile.ContentType;
                break;
            case 2:
                user.ProfileImage2Data = imageData;
                user.ProfileImage2ContentType = imageFile.ContentType;
                break;
            case 3:
                user.ProfileImage3Data = imageData;
                user.ProfileImage3ContentType = imageFile.ContentType;
                break;
        }

        user.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        await _userRepository.UpdateAsync(user);
        return imageData;
    }

    public async Task<bool> DeleteProfileImageAsync(Guid userId, int imageNumber)
    {
        if (imageNumber < 1 || imageNumber > _appSettings.MaxProfileImagesPerUser)
            return false;

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        switch (imageNumber)
        {
            case 1:
                user.ProfileImage1Data = null;
                user.ProfileImage1ContentType = null;
                break;
            case 2:
                user.ProfileImage2Data = null;
                user.ProfileImage2ContentType = null;
                break;
            case 3:
                user.ProfileImage3Data = null;
                user.ProfileImage3ContentType = null;
                break;
        }

        user.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<(byte[]? data, string? contentType)> GetProfileImageAsync(Guid userId, int imageNumber)
    {
        if (imageNumber < 1 || imageNumber > _appSettings.MaxProfileImagesPerUser)
            return (null, null);

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return (null, null);

        return imageNumber switch
        {
            1 => (user.ProfileImage1Data, user.ProfileImage1ContentType),
            2 => (user.ProfileImage2Data, user.ProfileImage2ContentType),
            3 => (user.ProfileImage3Data, user.ProfileImage3ContentType),
            _ => (null, null)
        };
    }

    public async Task<List<int>> GetAvailableImageSlotsAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return new List<int>();

        var availableSlots = new List<int>();
        
        if (user.ProfileImage1Data == null || user.ProfileImage1Data.Length == 0)
            availableSlots.Add(1);
        
        if (user.ProfileImage2Data == null || user.ProfileImage2Data.Length == 0)
            availableSlots.Add(2);
        
        if (user.ProfileImage3Data == null || user.ProfileImage3Data.Length == 0)
            availableSlots.Add(3);

        return availableSlots;
    }

    public async Task<Dictionary<int, (byte[] data, string contentType)>> GetAllProfileImagesAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return new Dictionary<int, (byte[] data, string contentType)>();

        var allImages = new Dictionary<int, (byte[] data, string contentType)>();

        if (user.ProfileImage1Data != null && user.ProfileImage1Data.Length > 0 && !string.IsNullOrEmpty(user.ProfileImage1ContentType))
        {
            allImages.Add(1, (user.ProfileImage1Data, user.ProfileImage1ContentType));
        }

        if (user.ProfileImage2Data != null && user.ProfileImage2Data.Length > 0 && !string.IsNullOrEmpty(user.ProfileImage2ContentType))
        {
            allImages.Add(2, (user.ProfileImage2Data, user.ProfileImage2ContentType));
        }

        if (user.ProfileImage3Data != null && user.ProfileImage3Data.Length > 0 && !string.IsNullOrEmpty(user.ProfileImage3ContentType))
        {
            allImages.Add(3, (user.ProfileImage3Data, user.ProfileImage3ContentType));
        }

        return allImages;
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
