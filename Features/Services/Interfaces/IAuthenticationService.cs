using global::DocumentManagement.Features.Data.Models;

namespace DocumentManagement.Features.Services.Interfaces;

public interface IAuthenticationService
{
    Task<User?> AuthenticateAsync(string username, string password);
    Task<User?> GetUserByIdAsync(int userId);
    Task<bool> IsUserAuthenticatedAsync();
    Task LogoutAsync();
}
