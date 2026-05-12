using DocumentManagement.Features.Repositories.Interfaces;
using DocumentManagement.Features.Data.Models;

namespace DocumentManagement.Features.Repositories.Interfaces;

public interface IUserRepository
{
	Task<User?> GetUserByIdAsync(int id);
	Task<User?> GetUserByEmailAsync(string email);
	Task<IEnumerable<User>> GetAllUsersAsync();
	Task<int> AddUserAsync(User user);
	Task<bool> UpdateUserAsync(User user);
	Task<bool> DeleteUserAsync(int id);
}
