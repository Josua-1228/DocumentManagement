using DocumentManagement.Features.Authentication.Interfaces;
using DocumentManagement.Features.Authentication.Models;
using DocumentManagement.Features.Repositories.Interfaces;
using DocumentManagement.Features.Data.Models;
using Microsoft.AspNetCore.Components;

namespace DocumentManagement.Features.Authentication.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly NavigationManager _navigationManager;
    private AuthState _authState = new();

    public AuthService(IUserRepository userRepository, NavigationManager navigationManager)
    {
        _userRepository = userRepository;
        _navigationManager = navigationManager;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return new LoginResponse 
                { 
                    Success = false, 
                    ErrorMessage = "Email and password are required" 
                };
            }

            // Find user by email
            var user = await _userRepository.GetUserByEmailAsync(loginRequest.Email);
            
            if (user == null || !user.IsActive)
            {
                return new LoginResponse 
                { 
                    Success = false, 
                    ErrorMessage = "Invalid email or password" 
                };
            }

            // For demo purposes, accept any non-empty password
            // In production, you would verify: BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)
            if (string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return new LoginResponse 
                { 
                    Success = false, 
                    ErrorMessage = "Password is required" 
                };
            }

            // Create authenticated user model
            var authenticatedUser = new AuthenticatedUser
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Username = user.Username,
                LoginTime = DateTime.UtcNow
            };

            // Update auth state
            _authState = new AuthState
            {
                IsAuthenticated = true,
                User = authenticatedUser,
                LastLoginTime = DateTime.UtcNow,
                SessionId = Guid.NewGuid().ToString()
            };

            return new LoginResponse 
            { 
                Success = true, 
                User = authenticatedUser,
                Token = _authState.SessionId
            };
        }
        catch (Exception ex)
        {
            return new LoginResponse 
            { 
                Success = false, 
                ErrorMessage = "An error occurred during login" 
            };
        }
    }

    public async Task LogoutAsync()
    {
        _authState = new AuthState();
        _navigationManager.NavigateTo("/login");
        await Task.CompletedTask;
    }

    public async Task<AuthState> GetCurrentAuthStateAsync()
    {
        return await Task.FromResult(_authState);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        return await Task.FromResult(_authState.IsAuthenticated);
    }

    public async Task<AuthenticatedUser?> GetCurrentUserAsync()
    {
        return await Task.FromResult(_authState.User);
    }
}
