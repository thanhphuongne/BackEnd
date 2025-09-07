namespace BackEnd.Application.DTOs.Auth;

public class RegisterResult
{
    public bool Succeeded { get; set; }
    public AuthResponse? AuthResponse { get; set; }
    public string? ErrorMessage { get; set; }
    public IEnumerable<string>? Errors { get; set; }

    public static RegisterResult Success(AuthResponse authResponse)
    {
        return new RegisterResult
        {
            Succeeded = true,
            AuthResponse = authResponse
        };
    }

    public static RegisterResult Failure(string errorMessage)
    {
        return new RegisterResult
        {
            Succeeded = false,
            ErrorMessage = errorMessage
        };
    }

    public static RegisterResult Failure(IEnumerable<string> errors)
    {
        return new RegisterResult
        {
            Succeeded = false,
            Errors = errors
        };
    }
}
