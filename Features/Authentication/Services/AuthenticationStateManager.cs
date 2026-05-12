using DocumentManagement.Features.Authentication.Interfaces;
using DocumentManagement.Features.Authentication.Models;

namespace DocumentManagement.Features.Authentication.Services;

public class AuthenticationStateManager
{
    private readonly IAuthService _authService;
    private AuthState _currentState = new();

    public AuthState CurrentState => _currentState;
    public bool IsAuthenticated => _currentState.IsAuthenticated;
    public AuthenticatedUser? CurrentUser => _currentState.User;

    public event Action<AuthState>? AuthStateChanged;

    public AuthenticationStateManager(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var response = await _authService.LoginAsync(loginRequest);
        
        if (response.Success)
        {
            _currentState = await _authService.GetCurrentAuthStateAsync();
            AuthStateChanged?.Invoke(_currentState);
        }
        
        return response;
    }

    public async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        _currentState = new AuthState();
        AuthStateChanged?.Invoke(_currentState);
    }

    public async Task RefreshAuthStateAsync()
    {
        _currentState = await _authService.GetCurrentAuthStateAsync();
        AuthStateChanged?.Invoke(_currentState);
    }
}
