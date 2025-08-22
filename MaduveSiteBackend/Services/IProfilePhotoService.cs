namespace MaduveSiteBackend.Services;

public interface IProfilePhotoService
{
    Task<byte[]> UploadPhotoAsync(Guid userId, IFormFile photoFile);
    Task<bool> DeletePhotoAsync(Guid userId);
    Task<(byte[]? data, string? contentType)> GetPhotoAsync(Guid userId);
}
