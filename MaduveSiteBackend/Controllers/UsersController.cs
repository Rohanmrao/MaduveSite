using Microsoft.AspNetCore.Mvc;
using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.DTOs;
using MaduveSiteBackend.Services;
using MaduveSiteBackend.Models.Authorization;

namespace MaduveSiteBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IProfilePhotoService _profilePhotoService;
    private readonly IUserRequestService _userRequestService;

    public UsersController(
        IUserService userService, 
        IProfilePhotoService profilePhotoService,
        IUserRequestService userRequestService)
    {
        _userService = userService;
        _profilePhotoService = profilePhotoService;
        _userRequestService = userRequestService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to retrieve users", details = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);
            return user is null ? NotFound(new { error = "User not found" }) : Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to retrieve user", details = ex.Message });
        }
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        try
        {
            var user = await _userService.GetByEmailAsync(email);
            return user is null ? NotFound(new { error = "User not found" }) : Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to retrieve user", details = ex.Message });
        }
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] CreateUserDto createUserDto)
    {
        if (createUserDto == null)
        {
            return BadRequest(new { error = "Request body is required" });
        }

        try
        {
            var userRequest = await _userRequestService.CreateUserRequestAsync(createUserDto);
            return CreatedAtAction(nameof(GetById), new { id = userRequest.Id }, new { 
                message = "Signup request submitted successfully. Please wait for admin approval.",
                requestId = userRequest.Id 
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to create signup request", details = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto updateUserDto)
    {
        if (updateUserDto == null)
        {
            return BadRequest(new { error = "Request body is required" });
        }

        try
        {
            var updated = await _userService.UpdateAsync(id, updateUserDto);
            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to update user", details = ex.Message });
        }
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeStatusDto changeStatusDto)
    {
        if (changeStatusDto == null)
        {
            return BadRequest(new { error = "Request body is required" });
        }

        try
        {
            await _userService.ChangeStatusAsync(id, changeStatusDto.Status);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to change user status", details = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [AdminPermission]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to delete user", details = ex.Message });
        }
    }

    [HttpPost("{id:guid}/photo")]
    public async Task<IActionResult> UploadPhoto(Guid id, IFormFile photo)
    {
        if (photo == null || photo.Length == 0)
            return BadRequest(new { error = "No file provided" });

        try
        {
            var photoData = await _profilePhotoService.UploadPhotoAsync(id, photo);
            return Ok(new { message = "Photo uploaded successfully", size = photoData.Length });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to upload photo", details = ex.Message });
        }
    }

    [HttpGet("{id:guid}/photo")]
    public async Task<IActionResult> GetPhoto(Guid id)
    {
        try
        {
            var (photoData, contentType) = await _profilePhotoService.GetPhotoAsync(id);
            
            if (photoData == null || contentType == null)
                return NotFound(new { error = "Photo not found" });

            return File(photoData, contentType);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to retrieve photo", details = ex.Message });
        }
    }

    [HttpDelete("{id:guid}/photo")]
    public async Task<IActionResult> DeletePhoto(Guid id)
    {
        try
        {
            var deleted = await _profilePhotoService.DeletePhotoAsync(id);
            if (!deleted)
                return NotFound(new { error = "Photo not found" });
            
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to delete photo", details = ex.Message });
        }
    }
}