using BackEnd.Application.Common.Interfaces;
using BackEnd.Application.DTOs.Match;
using BackEnd.Domain.Entities;
using BackEnd.Domain.Enums;
using BackEnd.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.Infrastructure.Services;

public class MatchService : IMatchService
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<MatchService> _logger;

    public MatchService(IApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<MatchService> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<List<MatchDto>> GetAllMatchesAsync()
    {
        _logger.LogInformation("🏆 Getting all matches");

        var matches = await _context.Matches
            .Include(m => m.Organizer)
            .Include(m => m.Field)
            .Where(m => m.Status != MatchStatus.Cancelled)
            .OrderByDescending(m => m.Created)
            .Select(m => new MatchDto
            {
                Id = m.Id,
                Title = m.Title,
                SportType = m.SportType.ToString(),
                StartTime = m.StartTime,
                EndTime = m.EndTime,
                Location = m.Field.FieldName,
                Address = m.Field.Address ?? m.Field.Venue.Address,
                MaxParticipants = m.MaxPlayers,
                CurrentPlayers = m.CurrentPlayers,
                SkillLevel = m.SkillLevel.ToString(),
                Description = m.Description,
                Status = m.Status.ToString(),
                Privacy = m.Privacy.ToString(),
                OrganizerName = m.Organizer.FullName,
                Phone = m.Organizer.Phone,
                Facebook = m.Organizer.Facebook,
                Created = m.Created.DateTime
            })
            .ToListAsync();

        _logger.LogInformation("✅ Found {Count} matches", matches.Count);
        return matches;
    }

    public async Task<List<MatchDto>> GetUserMatchesAsync(int userId)
    {
        _logger.LogInformation("🏆 Getting matches for user {UserId}", userId);

        var matches = await _context.Matches
            .Include(m => m.Organizer)
            .Include(m => m.Field)
            .Where(m => m.OrganizerId == userId || m.Participants.Any(p => p.UserId == userId))
            .OrderByDescending(m => m.Created)
            .Select(m => new MatchDto
            {
                Id = m.Id,
                Title = m.Title,
                SportType = m.SportType.ToString(),
                StartTime = m.StartTime,
                EndTime = m.EndTime,
                Location = m.Field.FieldName,
                Address = m.Field.Address ?? m.Field.Venue.Address,
                MaxParticipants = m.MaxPlayers,
                CurrentPlayers = m.CurrentPlayers,
                SkillLevel = m.SkillLevel.ToString(),
                Description = m.Description,
                Status = m.Status.ToString(),
                Privacy = m.Privacy.ToString(),
                OrganizerName = m.Organizer.FullName,
                Phone = m.Organizer.Phone,
                Facebook = m.Organizer.Facebook,
                Created = m.Created.DateTime
            })
            .ToListAsync();

        _logger.LogInformation("✅ Found {Count} matches for user {UserId}", matches.Count, userId);
        return matches;
    }

    public async Task<MatchDto?> CreateMatchAsync(int organizerId, CreateMatchRequest request)
    {
        _logger.LogInformation("📝 Creating match for user {UserId}", organizerId);

        // Parse sport
        if (!Enum.TryParse<SportType>(request.Sport, true, out var sportType))
        {
            _logger.LogWarning("❌ Invalid sport type: {Sport}", request.Sport);
            return null;
        }

        // Parse skill level
        if (!Enum.TryParse<SkillLevel>(request.SkillLevel, true, out var skillLevel))
        {
            _logger.LogWarning("❌ Invalid skill level: {SkillLevel}", request.SkillLevel);
            return null;
        }

        // Parse date and time
        if (!DateTime.TryParse(request.Date, out var date) || !TimeSpan.TryParse(request.Time, out var time))
        {
            _logger.LogWarning("❌ Invalid date or time format");
            return null;
        }

        var startTime = date.Date + time;
        var endTime = startTime.AddHours(1); // Assume 1 hour duration

        // Find a suitable field (for simplicity, pick the first active field of that sport)
        var field = await _context.Fields
            .FirstOrDefaultAsync(f => f.SportType == sportType && f.IsActive);

        if (field == null)
        {
            _logger.LogWarning("❌ No available field for sport {Sport}", request.Sport);
            return null;
        }

        var match = new Match
        {
            Title = request.Title,
            SportType = sportType,
            StartTime = startTime,
            EndTime = endTime,
            FieldId = field.Id,
            MaxPlayers = request.MaxParticipants,
            SkillLevel = skillLevel,
            Description = request.Description,
            Status = MatchStatus.Open,
            Privacy = MatchPrivacy.Public,
            OrganizerId = organizerId,
            Phone = request.Phone,
            Facebook = request.Facebook
        };

        _context.Matches.Add(match);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Add organizer as participant
        var participant = new MatchParticipant
        {
            MatchId = match.Id,
            UserId = organizerId,
            Status = ParticipantStatus.Confirmed,
            JoinedAt = DateTime.UtcNow
        };

        _context.MatchParticipants.Add(participant);
        await _context.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation("✅ Match created successfully: ID {MatchId}", match.Id);
        return await GetMatchByIdAsync(match.Id);
    }

    public async Task<bool> JoinMatchAsync(int matchId, int userId)
    {
        _logger.LogInformation("👥 User {UserId} joining match {MatchId}", userId, matchId);

        var match = await _context.Matches.FindAsync(matchId);
        if (match == null || match.Status != MatchStatus.Open)
        {
            _logger.LogWarning("❌ Match not found or not open: {MatchId}", matchId);
            return false;
        }

        var existingParticipant = await _context.MatchParticipants
            .FirstOrDefaultAsync(p => p.MatchId == matchId && p.UserId == userId);

        if (existingParticipant != null)
        {
            _logger.LogWarning("❌ User already participating in match: {MatchId}", matchId);
            return false;
        }

        var participant = new MatchParticipant
        {
            MatchId = matchId,
            UserId = userId,
            Status = match.Privacy == MatchPrivacy.Public ? ParticipantStatus.Confirmed : ParticipantStatus.Pending,
            JoinedAt = DateTime.UtcNow
        };

        _context.MatchParticipants.Add(participant);
        await _context.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation("✅ User {UserId} joined match {MatchId}", userId, matchId);
        return true;
    }

    public async Task<bool> ApproveJoinRequestAsync(int matchId, string userIdStr, int organizerId)
    {
        _logger.LogInformation("✅ Approving join request for user {UserId} in match {MatchId}", userIdStr, matchId);

        if (!int.TryParse(userIdStr, out var userId))
        {
            _logger.LogWarning("❌ Invalid user ID: {UserId}", userIdStr);
            return false;
        }

        var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == matchId && m.OrganizerId == organizerId);
        if (match == null)
        {
            _logger.LogWarning("❌ Match not found or user not organizer: {MatchId}", matchId);
            return false;
        }

        var participant = await _context.MatchParticipants
            .FirstOrDefaultAsync(p => p.MatchId == matchId && p.UserId == userId);

        if (participant == null || participant.Status != ParticipantStatus.Pending)
        {
            _logger.LogWarning("❌ Participant not found or not pending: {UserId}", userId);
            return false;
        }

        participant.Status = ParticipantStatus.Confirmed;
        await _context.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation("✅ Join request approved for user {UserId} in match {MatchId}", userId, matchId);
        return true;
    }

    public async Task<bool> RejectJoinRequestAsync(int matchId, string userIdStr, int organizerId)
    {
        _logger.LogInformation("❌ Rejecting join request for user {UserId} in match {MatchId}", userIdStr, matchId);

        if (!int.TryParse(userIdStr, out var userId))
        {
            _logger.LogWarning("❌ Invalid user ID: {UserId}", userIdStr);
            return false;
        }

        var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == matchId && m.OrganizerId == organizerId);
        if (match == null)
        {
            _logger.LogWarning("❌ Match not found or user not organizer: {MatchId}", matchId);
            return false;
        }

        var participant = await _context.MatchParticipants
            .FirstOrDefaultAsync(p => p.MatchId == matchId && p.UserId == userId);

        if (participant == null)
        {
            _logger.LogWarning("❌ Participant not found: {UserId}", userId);
            return false;
        }

        _context.MatchParticipants.Remove(participant);
        await _context.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation("✅ Join request rejected for user {UserId} in match {MatchId}", userId, matchId);
        return true;
    }

    public async Task<bool> CancelMatchAsync(int matchId, int organizerId)
    {
        _logger.LogInformation("❌ Cancelling match {MatchId} by organizer {OrganizerId}", matchId, organizerId);

        var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == matchId && m.OrganizerId == organizerId);
        if (match == null)
        {
            _logger.LogWarning("❌ Match not found or user not organizer: {MatchId}", matchId);
            return false;
        }

        match.Status = MatchStatus.Cancelled;
        await _context.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation("✅ Match {MatchId} cancelled", matchId);
        return true;
    }

    public async Task<bool> DeleteMatchAsync(int matchId, int organizerId)
    {
        _logger.LogInformation("🗑️ Deleting match {MatchId} by organizer {OrganizerId}", matchId, organizerId);

        var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == matchId && m.OrganizerId == organizerId);
        if (match == null)
        {
            _logger.LogWarning("❌ Match not found or user not organizer: {MatchId}", matchId);
            return false;
        }

        _context.Matches.Remove(match);
        await _context.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation("✅ Match {MatchId} deleted", matchId);
        return true;
    }

    public async Task<bool> LeaveMatchAsync(int matchId, int userId)
    {
        _logger.LogInformation("👋 User {UserId} leaving match {MatchId}", userId, matchId);

        var participant = await _context.MatchParticipants
            .FirstOrDefaultAsync(p => p.MatchId == matchId && p.UserId == userId);

        if (participant == null)
        {
            _logger.LogWarning("❌ User not participating in match: {MatchId}", matchId);
            return false;
        }

        _context.MatchParticipants.Remove(participant);
        await _context.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation("✅ User {UserId} left match {MatchId}", userId, matchId);
        return true;
    }

    public async Task<Domain.Entities.User?> GetUserByIdentityIdAsync(string identityId)
    {
        _logger.LogInformation("🔍 Looking up user by Identity ID: {IdentityId}", identityId);

        var identityUser = await _userManager.FindByIdAsync(identityId);
        if (identityUser == null)
        {
            _logger.LogWarning("❌ Identity user not found: {IdentityId}", identityId);
            return null;
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == identityUser.Email);

        if (user == null)
        {
            _logger.LogWarning("❌ Business user not found for email: {Email}", identityUser.Email);
            return null;
        }

        _logger.LogInformation("✅ Found user: {UserName} ({Email})", user.FullName, user.Email);
        return user;
    }

    private async Task<MatchDto?> GetMatchByIdAsync(int matchId)
    {
        var match = await _context.Matches
            .Include(m => m.Organizer)
            .Include(m => m.Field)
            .Where(m => m.Id == matchId)
            .Select(m => new MatchDto
            {
                Id = m.Id,
                Title = m.Title,
                SportType = m.SportType.ToString(),
                StartTime = m.StartTime,
                EndTime = m.EndTime,
                Location = m.Field.FieldName,
                Address = m.Field.Address ?? m.Field.Venue.Address,
                MaxParticipants = m.MaxPlayers,
                CurrentPlayers = m.CurrentPlayers,
                SkillLevel = m.SkillLevel.ToString(),
                Description = m.Description,
                Status = m.Status.ToString(),
                Privacy = m.Privacy.ToString(),
                OrganizerName = m.Organizer.FullName,
                Phone = m.Organizer.Phone,
                Facebook = m.Organizer.Facebook,
                Created = m.Created.DateTime
            })
            .FirstOrDefaultAsync();

        return match;
    }
}