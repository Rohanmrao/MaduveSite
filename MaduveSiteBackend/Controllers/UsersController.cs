using Microsoft.AspNetCore.Mvc;
using MaduveSiteBackend.Models;
using MaduveSiteBackend.Models.DTOs;
using MaduveSiteBackend.Services;

namespace MaduveSiteBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IProfilePhotoService _profilePhotoService;

    public UsersController(IUserService userService, IProfilePhotoService profilePhotoService)
    {
        _userService = userService;
        _profilePhotoService = profilePhotoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
    {
        var created = await _userService.CreateAsync(createUserDto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            var updated = await _userService.UpdateAsync(id, updateUserDto);
            return Ok(updated);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeStatusDto changeStatusDto)
    {
        await _userService.ChangeStatusAsync(id, changeStatusDto.Status);
        return NoContent();
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
    }

    [HttpGet("{id:guid}/photo")]
    public async Task<IActionResult> GetPhoto(Guid id)
    {
        var (photoData, contentType) = await _profilePhotoService.GetPhotoAsync(id);
        
        if (photoData == null || contentType == null)
            return NotFound();

        return File(photoData, contentType);
    }

    [HttpDelete("{id:guid}/photo")]
    public async Task<IActionResult> DeletePhoto(Guid id)
    {
        var deleted = await _profilePhotoService.DeletePhotoAsync(id);
        if (!deleted)
            return NotFound();
        
        return NoContent();
    }
}