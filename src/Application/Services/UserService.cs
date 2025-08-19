using BackEnd.Application.Common.Interfaces;
using BackEnd.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Application.Services;

public class UserService
{
    private readonly IApplicationDbContext _context;

    public UserService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone,
                DisplayName = u.DisplayName,
                AccountType = u.AccountType.ToString()
            })
            .ToListAsync();
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            DisplayName = user.DisplayName,
            AccountType = user.AccountType.ToString()
        };
    }
}
