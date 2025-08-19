namespace BackEnd.Application.DTOs;

public class CustomerDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string Role { get; set; } = null!;
}
