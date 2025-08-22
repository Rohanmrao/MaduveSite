namespace MaduveSiteBackend.Configuration;

public class AppSettings
{
    public const string SectionName = "AppSettings";
    
    public string ApplicationUrl { get; set; } = "http://localhost:5000";
    public int MaxImageSizeBytes { get; set; } = 5 * 1024 * 1024; // 5MB
    public int MaxProfileImagesPerUser { get; set; } = 3;
    public string[] AllowedImageExtensions { get; set; } = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    public string[] AllowedImageMimeTypes { get; set; } = { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
    public int MinimumPasswordLength { get; set; } = 6;
    public string DefaultPasswordHash { get; set; } = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
}
