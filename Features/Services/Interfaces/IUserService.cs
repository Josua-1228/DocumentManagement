using DocumentManagement.Features.Data.Models;

namespace DocumentManagement.Features.Services.Interfaces;

public interface IUserService
{
	Task<User?> GetUserAsync(int userId);
	Task<User?> GetUserByEmailAsync(string email);
	Task<IEnumerable<User>> GetAllUsersAsync();
	Task<int> CreateUserAsync(User user);
	Task<bool> UpdateUserAsync(User user);
	Task<bool> DeleteUserAsync(int userId);
}
