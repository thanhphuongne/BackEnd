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
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("🔐 Login attempt for email: {Email}", request.Email);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("❌ Login failed - Invalid model state for email: {Email}", request.Email);
            return BadRequest(ModelState);
        }

        var result = await _authService.LoginAsync(request);
        if (result == null)
        {
            _logger.LogWarning("❌ Login failed - Invalid credentials for email: {Email}", request.Email);
            return Unauthorized(new { message = "Invalid email or password" });
        }

        _logger.LogInformation("✅ Login successful for email: {Email}, User: {UserName}",
            request.Email, result.User.FullName);
        return Ok(result);
    }

    /// <summary>
    /// Register a new user account
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation("📝 Registration attempt for email: {Email}, Name: {FullName}",
            request.Email, request.FullName);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("❌ Registration failed - Invalid model state for email: {Email}", request.Email);
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterAsync(request);
        if (result == null)
        {
            _logger.LogWarning("❌ Registration failed - User already exists: {Email}", request.Email);
            return BadRequest(new { message = "User with this email already exists" });
        }

        _logger.LogInformation("✅ Registration successful for email: {Email}, User: {UserName}",
            request.Email, result.User.FullName);
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
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        _logger.LogInformation("👤 Profile request for user: {Email} (ID: {UserId})", userEmail, userId);

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("❌ Profile request failed - No user ID in token");
            return Unauthorized();
        }

        var profile = await _authService.GetUserProfileAsync(userId);
        if (profile == null)
        {
            _logger.LogWarning("❌ Profile not found for user ID: {UserId}", userId);
            return NotFound(new { message = "User profile not found" });
        }

        _logger.LogInformation("✅ Profile retrieved for user: {Email}", profile.Email);
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
