using Microsoft.AspNetCore.Mvc;
using MaduveSiteBackend.Services;
using MaduveSiteBackend.Configuration;

namespace MaduveSiteBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileImageController : ControllerBase
{
    private readonly IProfileImageService _profileImageService;
    private readonly AppSettings _appSettings;

    public ProfileImageController(IProfileImageService profileImageService, AppSettings appSettings)
    {
        _profileImageService = profileImageService;
        _appSettings = appSettings;
    }

    [HttpPost("{userId:guid}/upload/{imageNumber:int}")]
    public async Task<IActionResult> UploadProfileImage(Guid userId, int imageNumber, IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            return BadRequest(new { error = "Image file is required" });
        }

        if (imageNumber < 1 || imageNumber > _appSettings.MaxProfileImagesPerUser)
        {
            return BadRequest(new { error = $"Image number must be between 1 and {_appSettings.MaxProfileImagesPerUser}" });
        }

        try
        {
            var imageData = await _profileImageService.UploadProfileImageAsync(userId, imageNumber, image);
            return Ok(new { message = $"Profile image {imageNumber} uploaded successfully", size = imageData.Length });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to upload profile image", details = ex.Message });
        }
    }

    [HttpGet("{userId:guid}/image/{imageNumber:int}")]
    public async Task<IActionResult> GetProfileImage(Guid userId, int imageNumber)
    {
        if (imageNumber < 1 || imageNumber > _appSettings.MaxProfileImagesPerUser)
        {
            return BadRequest(new { error = $"Image number must be between 1 and {_appSettings.MaxProfileImagesPerUser}" });
        }

        try
        {
            var (imageData, contentType) = await _profileImageService.GetProfileImageAsync(userId, imageNumber);

            if (imageData == null || contentType == null)
            {
                return NotFound(new { error = $"Profile image {imageNumber} not found" });
            }

            return File(imageData, contentType);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to retrieve profile image", details = ex.Message });
        }
    }

    [HttpGet("{userId:guid}/all")]
    public async Task<IActionResult> GetAllProfileImages(Guid userId)
    {
        try
        {
            var allImages = await _profileImageService.GetAllProfileImagesAsync(userId);
            
            if (!allImages.Any())
            {
                return NotFound(new { error = "No profile images found for this user" });
            }

            var response = new
            {
                userId = userId,
                totalImages = allImages.Count,
                images = allImages.Select(kvp => new
                {
                    imageNumber = kvp.Key,
                    contentType = kvp.Value.contentType,
                    size = kvp.Value.data.Length,
                    imageUrl = $"/api/profile-image/{userId}/image/{kvp.Key}"
                }).ToList()
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to retrieve profile images", details = ex.Message });
        }
    }



    [HttpDelete("{userId:guid}/image/{imageNumber:int}")]
    public async Task<IActionResult> DeleteProfileImage(Guid userId, int imageNumber)
    {
        if (imageNumber < 1 || imageNumber > _appSettings.MaxProfileImagesPerUser)
        {
            return BadRequest(new { error = $"Image number must be between 1 and {_appSettings.MaxProfileImagesPerUser}" });
        }

        try
        {
            var deleted = await _profileImageService.DeleteProfileImageAsync(userId, imageNumber);

            if (!deleted)
            {
                return NotFound(new { error = $"Profile image {imageNumber} not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to delete profile image", details = ex.Message });
        }
    }

    [HttpGet("{userId:guid}/available-slots")]
    public async Task<IActionResult> GetAvailableImageSlots(Guid userId)
    {
        try
        {
            var availableSlots = await _profileImageService.GetAvailableImageSlotsAsync(userId);
            return Ok(new { availableSlots });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to get available image slots", details = ex.Message });
        }
    }
}
