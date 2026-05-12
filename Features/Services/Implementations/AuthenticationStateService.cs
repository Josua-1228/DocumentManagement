using global::DocumentManagement.Features.Data.Models;

namespace DocumentManagement.Features.Services.Implementations;

public class AuthenticationStateService
{
    private User? _currentUser;
    private bool _isAuthenticated = false;

    public User? CurrentUser => _currentUser;
    public bool IsAuthenticated => _isAuthenticated;

    public event Action<User?>? UserChanged;

    public void SetUser(User? user)
    {
        _currentUser = user;
        _isAuthenticated = user != null;
        UserChanged?.Invoke(user);
    }

    public void ClearUser()
    {
        _currentUser = null;
        _isAuthenticated = false;
        UserChanged?.Invoke(null);
    }
}
