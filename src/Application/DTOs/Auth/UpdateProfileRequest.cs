namespace BackEnd.Application.DTOs.Auth;

public class UpdateProfileRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Bio { get; set; }
    public string? Country { get; set; }
    public string? CityState { get; set; }
    public string? PostalCode { get; set; }
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }
    public string? Linkedin { get; set; }
    public string? Instagram { get; set; }
}