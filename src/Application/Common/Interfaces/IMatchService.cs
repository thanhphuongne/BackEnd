using BackEnd.Application.DTOs.Match;

namespace BackEnd.Application.Common.Interfaces;

public interface IMatchService
{
    Task<List<MatchDto>> GetAllMatchesAsync();
    Task<List<MatchDto>> GetUserMatchesAsync(int userId);
    Task<MatchDto?> CreateMatchAsync(int organizerId, CreateMatchRequest request);
    Task<bool> JoinMatchAsync(int matchId, int userId);
    Task<bool> ApproveJoinRequestAsync(int matchId, string userId, int organizerId);
    Task<bool> RejectJoinRequestAsync(int matchId, string userId, int organizerId);
    Task<bool> CancelMatchAsync(int matchId, int organizerId);
    Task<bool> DeleteMatchAsync(int matchId, int organizerId);
    Task<bool> LeaveMatchAsync(int matchId, int userId);
    Task<Domain.Entities.User?> GetUserByIdentityIdAsync(string identityId);
}