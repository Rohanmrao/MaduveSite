using Microsoft.AspNetCore.Mvc;
using MaduveSiteBackend.Models.DTOs;
using MaduveSiteBackend.Services;

namespace MaduveSiteBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("user")]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginDto loginDto)
    {
        if (loginDto == null)
        {
            return BadRequest(new { error = "Request body is required" });
        }

        try
        {
            var result = await _loginService.LoginUserAsync(loginDto);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return Unauthorized(result);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Login failed", details = ex.Message });
        }
    }

    [HttpPost("admin")]
    public async Task<IActionResult> LoginAdmin([FromBody] AdminLoginDto loginDto)
    {
        if (loginDto == null)
        {
            return BadRequest(new { error = "Request body is required" });
        }

        try
        {
            var result = await _loginService.LoginAdminAsync(loginDto);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return Unauthorized(result);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Login failed", details = ex.Message });
        }
    }

    [HttpGet("status/{email}")]
    public async Task<IActionResult> GetApplicationStatus(string email)
    {
        try
        {
            var status = await _loginService.GetApplicationStatusAsync(email);
            return Ok(status);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to get application status", details = ex.Message });
        }
    }
}
