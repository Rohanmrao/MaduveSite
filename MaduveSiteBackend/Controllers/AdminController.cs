using Microsoft.AspNetCore.Mvc;
using MaduveSiteBackend.Services;
using MaduveSiteBackend.Models.DTOs;

namespace MaduveSiteBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var stats = await _adminService.GetDashboardStatsAsync();
        return Ok(stats);
    }

    [HttpGet("requests/pending")]
    public async Task<IActionResult> GetPendingRequests()
    {
        var requests = await _adminService.GetPendingRequestsAsync();
        return Ok(requests);
    }

    [HttpGet("requests/{id}")]
    public async Task<IActionResult> GetPendingRequestById(Guid id)
    {
        var request = await _adminService.GetPendingRequestByIdAsync(id);
        if (request == null)
            return NotFound();
        return Ok(request);
    }

    [HttpPost("requests/{id}/approve")]
    public async Task<IActionResult> ApproveUser(Guid id, [FromBody] Guid adminId)
    {
        var result = await _adminService.ApproveUserAsync(id, adminId);
        return Ok(result);
    }

    [HttpPost("requests/{id}/reject")]
    public async Task<IActionResult> RejectUser(Guid id, [FromBody] Guid adminId)
    {
        var result = await _adminService.RejectUserAsync(id, adminId);
        return Ok(result);
    }

    [HttpDelete("requests/pending")]
    public async Task<IActionResult> DeleteAllPendingRequests()
    {
        await _adminService.DeletePendingRequestsAsync();
        return NoContent();
    }

    [HttpDelete("requests/{id}")]
    public async Task<IActionResult> DeletePendingRequest(Guid id)
    {
        await _adminService.DeletePendingRequestByIdAsync(id);
        return NoContent();
    }

    [HttpPost("signup")]
    public async Task<IActionResult> AddAdmin([FromBody] AdminSignupDto adminDto)
    {
        try
        {
            var admin = await _adminService.AddAdminAsync(adminDto);
            return CreatedAtAction(nameof(GetAdminById), new { id = admin.Id }, admin);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdminById(Guid id)
    {
        var admin = await _adminService.GetAdminByIdAsync(id);
        if (admin == null)
            return NotFound();
        return Ok(admin);
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetAdminByEmail(string email)
    {
        var admin = await _adminService.GetAdminDetailsByEmailAsync(email);
        if (admin == null)
            return NotFound(new { error = "Admin not found" });
        return Ok(admin);
    }

    [HttpGet("auth-level/{userId}")]
    public async Task<IActionResult> GetAuthLevel(Guid userId)
    {
        var authLevel = await _adminService.GetAuthLevelAsync(userId);
        return Ok(new { userId = userId, authLevel = authLevel });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAdmins()
    {
        var admins = await _adminService.GetAllAdminsAsync();
        return Ok(admins);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdmin(Guid id, [FromBody] AdminUpdateDto adminDto)
    {
        try
        {
            var admin = await _adminService.UpdateAdminAsync(id, adminDto);
            return Ok(admin);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveAdmin(Guid id)
    {
        await _adminService.RemoveAdminAsync(id);
        return NoContent();
    }
}