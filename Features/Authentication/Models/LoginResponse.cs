namespace DocumentManagement.Features.Authentication.Models;

public class LoginResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? ErrorMessage { get; set; }
    public AuthenticatedUser? User { get; set; }
}

public class AuthenticatedUser
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public DateTime LoginTime { get; set; } = DateTime.UtcNow;
}
