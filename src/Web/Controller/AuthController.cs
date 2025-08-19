using BackEnd.Application.Common.Interfaces;
using BackEnd.Application.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackEnd.Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(request);
        if (result == null)
            return Unauthorized(new { message = "Invalid email or password" });

        return Ok(result);
    }

    /// <summary>
    /// Register a new user account
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(request);
        if (result == null)
            return BadRequest(new { message = "User with this email already exists" });

        return Ok(result);
    }

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RefreshTokenAsync(request);
        if (result == null)
            return Unauthorized(new { message = "Invalid refresh token" });

        return Ok(result);
    }

    /// <summary>
    /// Get current user profile
    /// </summary>
    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var profile = await _authService.GetUserProfileAsync(userId);
        if (profile == null)
            return NotFound(new { message = "User profile not found" });

        return Ok(profile);
    }

    /// <summary>
    /// Change user password
    /// </summary>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _authService.ChangePasswordAsync(userId, request);
        if (!result)
            return BadRequest(new { message = "Failed to change password. Please check your current password." });

        return Ok(new { message = "Password changed successfully" });
    }

    /// <summary>
    /// Logout (client-side token invalidation)
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        // In a JWT-based system, logout is typically handled client-side by removing the token
        // For server-side logout, you would need to implement a token blacklist
        return Ok(new { message = "Logged out successfully" });
    }

    /// <summary>
    /// Check if user is authenticated
    /// </summary>
    [HttpGet("check")]
    [Authorize]
    public IActionResult CheckAuth()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var name = User.FindFirst(ClaimTypes.Name)?.Value;
        var accountType = User.FindFirst("AccountType")?.Value;

        return Ok(new
        {
            isAuthenticated = true,
            userId,
            email,
            name,
            accountType
        });
    }
}
