namespace DocumentManagement.Features.Authentication.Models;

public class AuthState
{
    public bool IsAuthenticated { get; set; }
    public AuthenticatedUser? User { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public string? SessionId { get; set; }
}
