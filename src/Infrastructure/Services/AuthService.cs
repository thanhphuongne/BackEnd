using BackEnd.Application.Common.Interfaces;
using BackEnd.Application.DTOs.Auth;
using BackEnd.Domain.Entities;
using BackEnd.Domain.Enums;
using BackEnd.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BackEnd.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IApplicationDbContext context,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation("🔍 AuthService: Looking up user by email: {Email}", request.Email);

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogWarning("❌ AuthService: User not found: {Email}", request.Email);
            return null;
        }

        _logger.LogInformation("🔐 AuthService: Checking password for user: {Email}", request.Email);
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("❌ AuthService: Password check failed for user: {Email}", request.Email);
            return null;
        }

        _logger.LogInformation("📝 AuthService: Updating last login for user: {Email}", request.Email);
        // Update last login
        var appUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (appUser != null)
        {
            appUser.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        _logger.LogInformation("🎫 AuthService: Generating JWT token for user: {Email}", request.Email);
        return await GenerateAuthResponseAsync(user, appUser);
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        _logger.LogInformation("📝 AuthService: Registration attempt for email: {Email}, Name: {FullName}",
            request.Email, request.FullName);

        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("❌ AuthService: User already exists: {Email}", request.Email);
            return null;
        }

        _logger.LogInformation("👤 AuthService: Creating Identity user for: {Email}", request.Email);
        // Create Identity user
        var identityUser = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true // For simplicity, auto-confirm emails
        };

        var result = await _userManager.CreateAsync(identityUser, request.Password);
        if (!result.Succeeded)
        {
            _logger.LogError("❌ AuthService: Failed to create Identity user for: {Email}. Errors: {Errors}",
                request.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
            return null;
        }

        _logger.LogInformation("💾 AuthService: Creating business user record for: {Email}", request.Email);
        // Create business user
        var appUser = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            DisplayName = request.DisplayName,
            Phone = request.Phone,
            DateOfBirth = request.DateOfBirth,
            AccountType = request.AccountType,
            PasswordHash = identityUser.PasswordHash!, // Store the hashed password
            LastLogin = DateTime.UtcNow
        };

        _context.Users.Add(appUser);
        await _context.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation("✅ AuthService: User registration completed successfully for: {Email}", request.Email);
        return await GenerateAuthResponseAsync(identityUser, appUser);
    }

    public async Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request)
    {
        // For simplicity, we'll validate the refresh token and generate new tokens
        // In production, you should store refresh tokens in database with expiration
        var principal = GetPrincipalFromExpiredToken(request.RefreshToken);
        if (principal == null)
            return null;

        var email = principal.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
            return null;

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return null;

        var appUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return await GenerateAuthResponseAsync(user, appUser);
    }

    public async Task<UserProfileDto?> GetUserProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return null;

        var appUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (appUser == null)
            return null;

        return new UserProfileDto
        {
            Id = user.Id,
            Email = user.Email!,
            FullName = appUser.FullName,
            DisplayName = appUser.DisplayName,
            Phone = appUser.Phone,
            DateOfBirth = appUser.DateOfBirth,
            ProfilePhotoUrl = appUser.ProfilePhotoUrl,
            AccountType = appUser.AccountType.ToString(),
            LastLogin = appUser.LastLogin,
            CreatedAt = appUser.Created.DateTime
        };
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        return result.Succeeded;
    }

    private Task<AuthResponse> GenerateAuthResponseAsync(ApplicationUser identityUser, User? appUser)
    {
        var token = GenerateJwtToken(identityUser, appUser);
        var refreshToken = GenerateRefreshToken();

        return Task.FromResult(new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(24), // Token expires in 24 hours
            User = new UserProfileDto
            {
                Id = identityUser.Id,
                Email = identityUser.Email!,
                FullName = appUser?.FullName ?? identityUser.Email!,
                DisplayName = appUser?.DisplayName,
                Phone = appUser?.Phone,
                DateOfBirth = appUser?.DateOfBirth,
                ProfilePhotoUrl = appUser?.ProfilePhotoUrl,
                AccountType = appUser?.AccountType.ToString() ?? AccountType.Customer.ToString(),
                LastLogin = appUser?.LastLogin,
                CreatedAt = appUser?.Created.DateTime ?? DateTime.UtcNow
            }
        });
    }

    private string GenerateJwtToken(ApplicationUser identityUser, User? appUser)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!");
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, identityUser.Id),
            new(ClaimTypes.Email, identityUser.Email!),
            new(ClaimTypes.Name, appUser?.FullName ?? identityUser.Email!),
            new("AccountType", appUser?.AccountType.ToString() ?? AccountType.Customer.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"] ?? "SportsBookingAPI",
            Audience = jwtSettings["Audience"] ?? "SportsBookingClient"
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = false // We don't care about expiration for refresh
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            if (validatedToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
