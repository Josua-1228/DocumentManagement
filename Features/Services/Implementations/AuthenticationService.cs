using global::DocumentManagement.Features.Repositories.Interfaces;
using global::DocumentManagement.Features.Services.Interfaces;
using global::DocumentManagement.Features.Data.Models;
using global::DocumentManagement.Features.Authentication.Models;
using global::DocumentManagement.Features.Authentication.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using System.Text;

namespace DocumentManagement.Features.Services.Implementations;

public class AuthenticationService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly NavigationManager _navigationManager;
    private User? _currentUser;

    public AuthenticationService(IUserRepository userRepository, NavigationManager navigationManager)
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
                    ErrorMessage = "Email and password are required."
                };
            }

            // Validate email format
            if (!IsValidEmail(loginRequest.Email))
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Please enter a valid email address."
                };
            }

            // Find user by email
            var user = await _userRepository.GetUserByEmailAsync(loginRequest.Email);
            
            if (user == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Invalid email or password."
                };
            }

            if (!user.IsActive)
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Account is deactivated. Please contact administrator."
                };
            }

            // For demo purposes, accept any non-empty password for existing users
            // In production, you would verify: BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash)
            if (!string.IsNullOrEmpty(loginRequest.Password))
            {
                _currentUser = user;
                
                // Update user's last login time
                user.LastLoginAt = DateTime.UtcNow;
                await _userRepository.UpdateUserAsync(user);
                
                return new LoginResponse
                {
                    Success = true,
                    User = new AuthenticatedUser
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FullName = user.FullName,
                        Username = user.Email,
                        LoginTime = DateTime.UtcNow,
                        Role = user.Role ?? "User"
                    },
                    Token = Guid.NewGuid().ToString() // Simple token for demo
                };
            }
            else
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Invalid email or password."
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            return new LoginResponse
            {
                Success = false,
                ErrorMessage = "An error occurred during login. Please try again."
            };
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        try
        {
            // In a real application, you would hash the password and compare
            // For demo purposes, we'll use a simple validation
            var user = await _userRepository.GetUserByEmailAsync(username);
            
            if (user != null && user.IsActive)
            {
                // For demo purposes, accept any non-empty password for existing users
                // In production, you would verify: BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)
                _currentUser = user;
                return user;
            }
            
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Authentication error: {ex.Message}");
            return null;
        }
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _userRepository.GetUserByIdAsync(userId);
    }

    public Task<bool> IsUserAuthenticatedAsync()
    {
        return Task.FromResult(_currentUser != null);
    }

    public Task LogoutAsync()
    {
        _currentUser = null;
        _navigationManager.NavigateTo("/simple-login");
        return Task.CompletedTask;
    }

    public User? GetCurrentUser()
    {
        return _currentUser;
    }

    public void SetCurrentUser(User user)
    {
        _currentUser = user;
    }

    public async Task<AuthState> GetCurrentAuthStateAsync()
    {
        if (_currentUser != null)
        {
            return new AuthState
            {
                IsAuthenticated = true,
                User = new AuthenticatedUser
                {
                    Id = _currentUser.Id,
                    Email = _currentUser.Email,
                    FullName = _currentUser.FullName,
                    Username = _currentUser.Email,
                    LoginTime = DateTime.UtcNow
                }
            };
        }

        return new AuthState
        {
            IsAuthenticated = false,
            User = null
        };
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        return await IsUserAuthenticatedAsync();
    }

    public async Task<AuthenticatedUser?> GetCurrentUserAsync()
    {
        if (_currentUser != null)
        {
            return new AuthenticatedUser
            {
                Id = _currentUser.Id,
                Email = _currentUser.Email,
                FullName = _currentUser.FullName,
                Username = _currentUser.Email,
                LoginTime = DateTime.UtcNow
            };
        }

        return null;
    }
}
