using LostAndFound.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LostAndFound.Services.Interfaces;

namespace LostAndFound.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthServices _authservice;

    public AuthController(IAuthService authService)
    {
        authService = authService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authservice.LoginAsync(loginDto);
        if (result == null)
            return Unauthorized(new { message = "Invalid Email Or Password" });

                return Ok(result);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var result = await _authservice.RegisterAsync(registerDto);
        if (result == null)
            return BadRequest(new { message = "Registration FAILED.Email already in Use" });
        return Ok(result);
    }
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var email = User.FindFirstValue(ClaimTypes.Email);
    var name = User.FindFirstValue(ClaimTypes.Name);
    var roles = User.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
 return Ok(new
        {
        userId,
        email,
        name,
        roles
});

}
    
}
