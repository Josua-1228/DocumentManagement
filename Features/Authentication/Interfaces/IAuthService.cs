using DocumentManagement.Features.Authentication.Models;

namespace DocumentManagement.Features.Authentication.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    Task LogoutAsync();
    Task<AuthState> GetCurrentAuthStateAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<AuthenticatedUser?> GetCurrentUserAsync();
}
