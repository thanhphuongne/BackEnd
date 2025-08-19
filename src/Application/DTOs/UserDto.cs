namespace BackEnd.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? DisplayName { get; set; }
    public string AccountType { get; set; } = null!;
}
