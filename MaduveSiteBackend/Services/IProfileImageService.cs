using MaduveSiteBackend.Repositories;

namespace MaduveSiteBackend.Services;

public interface IProfileImageService
{
    Task<byte[]> UploadProfileImageAsync(Guid userId, int imageNumber, IFormFile imageFile);
    Task<bool> DeleteProfileImageAsync(Guid userId, int imageNumber);
    Task<(byte[]? data, string? contentType)> GetProfileImageAsync(Guid userId, int imageNumber);
    Task<List<int>> GetAvailableImageSlotsAsync(Guid userId);
    Task<Dictionary<int, (byte[] data, string contentType)>> GetAllProfileImagesAsync(Guid userId);
}
