using BackEnd.Application.Common.Interfaces;
using BackEnd.Application.DTOs.Match;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackEnd.Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly IMatchService _matchService;
    private readonly ILogger<MatchesController> _logger;

    public MatchesController(IMatchService matchService, ILogger<MatchesController> logger)
    {
        _matchService = matchService;
        _logger = logger;
    }

    /// <summary>
    /// Get all matches
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllMatches()
    {
        _logger.LogInformation("🏆 Getting all matches");
        var matches = await _matchService.GetAllMatchesAsync();
        return Ok(matches);
    }

    /// <summary>
    /// Get user's matches (requires authentication)
    /// </summary>
    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMyMatches()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var user = await _matchService.GetUserByIdentityIdAsync(userIdClaim);
        if (user == null)
            return Unauthorized();

        _logger.LogInformation("🏆 Getting matches for user: {Email}", user.Email);
        var matches = await _matchService.GetUserMatchesAsync(user.Id);
        return Ok(matches);
    }

    /// <summary>
    /// Create a new match (requires authentication)
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateMatch([FromBody] CreateMatchRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var user = await _matchService.GetUserByIdentityIdAsync(userIdClaim);
        if (user == null)
            return Unauthorized();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation("📝 Creating match for user: {Email}", user.Email);
        var match = await _matchService.CreateMatchAsync(user.Id, request);
        if (match == null)
            return BadRequest(new { message = "Failed to create match" });

        return CreatedAtAction(nameof(GetAllMatches), new { id = match.Id }, match);
    }

    /// <summary>
    /// Join a match (requires authentication)
    /// </summary>
    [HttpPost("{id}/join")]
    [Authorize]
    public async Task<IActionResult> JoinMatch(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var user = await _matchService.GetUserByIdentityIdAsync(userIdClaim);
        if (user == null)
            return Unauthorized();

        _logger.LogInformation("👥 User {Email} joining match {MatchId}", user.Email, id);
        var result = await _matchService.JoinMatchAsync(id, user.Id);
        if (!result)
            return BadRequest(new { message = "Failed to join match" });

        return Ok(new { message = "Joined match successfully" });
    }

    /// <summary>
    /// Approve join request (requires authentication, organizer only)
    /// </summary>
    [HttpPut("{id}/approve/{userId}")]
    [Authorize]
    public async Task<IActionResult> ApproveJoinRequest(int id, string userId)
    {
        var organizerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(organizerIdClaim))
            return Unauthorized();

        var organizer = await _matchService.GetUserByIdentityIdAsync(organizerIdClaim);
        if (organizer == null)
            return Unauthorized();

        _logger.LogInformation("✅ Organizer {Email} approving join request for user {UserId} in match {MatchId}", organizer.Email, userId, id);
        var result = await _matchService.ApproveJoinRequestAsync(id, userId, organizer.Id);
        if (!result)
            return BadRequest(new { message = "Failed to approve join request" });

        return Ok(new { message = "Join request approved" });
    }

    /// <summary>
    /// Reject join request (requires authentication, organizer only)
    /// </summary>
    [HttpPut("{id}/reject/{userId}")]
    [Authorize]
    public async Task<IActionResult> RejectJoinRequest(int id, string userId)
    {
        var organizerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(organizerIdClaim))
            return Unauthorized();

        var organizer = await _matchService.GetUserByIdentityIdAsync(organizerIdClaim);
        if (organizer == null)
            return Unauthorized();

        _logger.LogInformation("❌ Organizer {Email} rejecting join request for user {UserId} in match {MatchId}", organizer.Email, userId, id);
        var result = await _matchService.RejectJoinRequestAsync(id, userId, organizer.Id);
        if (!result)
            return BadRequest(new { message = "Failed to reject join request" });

        return Ok(new { message = "Join request rejected" });
    }

    /// <summary>
    /// Cancel a match (requires authentication, organizer only)
    /// </summary>
    [HttpPut("{id}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelMatch(int id)
    {
        var organizerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(organizerIdClaim))
            return Unauthorized();

        var organizer = await _matchService.GetUserByIdentityIdAsync(organizerIdClaim);
        if (organizer == null)
            return Unauthorized();

        _logger.LogInformation("❌ Organizer {Email} cancelling match {MatchId}", organizer.Email, id);
        var result = await _matchService.CancelMatchAsync(id, organizer.Id);
        if (!result)
            return BadRequest(new { message = "Failed to cancel match" });

        return Ok(new { message = "Match cancelled successfully" });
    }

    /// <summary>
    /// Delete a match (requires authentication, organizer only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteMatch(int id)
    {
        var organizerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(organizerIdClaim))
            return Unauthorized();

        var organizer = await _matchService.GetUserByIdentityIdAsync(organizerIdClaim);
        if (organizer == null)
            return Unauthorized();

        _logger.LogInformation("🗑️ Organizer {Email} deleting match {MatchId}", organizer.Email, id);
        var result = await _matchService.DeleteMatchAsync(id, organizer.Id);
        if (!result)
            return BadRequest(new { message = "Failed to delete match" });

        return Ok(new { message = "Match deleted successfully" });
    }

    /// <summary>
    /// Leave a match (requires authentication)
    /// </summary>
    [HttpPut("{id}/leave")]
    [Authorize]
    public async Task<IActionResult> LeaveMatch(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var user = await _matchService.GetUserByIdentityIdAsync(userIdClaim);
        if (user == null)
            return Unauthorized();

        _logger.LogInformation("👋 User {Email} leaving match {MatchId}", user.Email, id);
        var result = await _matchService.LeaveMatchAsync(id, user.Id);
        if (!result)
            return BadRequest(new { message = "Failed to leave match" });

        return Ok(new { message = "Left match successfully" });
    }
}