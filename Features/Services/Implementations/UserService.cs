using DocumentManagement.Features.Data.Models;
using DocumentManagement.Features.Repositories.Interfaces;
using DocumentManagement.Features.Services.Interfaces;

namespace DocumentManagement.Features.Services.Implementations;

public class UserService : IUserService
{
	private readonly IUserRepository _repository;
	private readonly ILogger<UserService> _logger;

	public UserService(IUserRepository repository, ILogger<UserService> logger)
	{
		_repository = repository;
		_logger = logger;
	}

	public async Task<User?> GetUserAsync(int userId)
	{
		return await _repository.GetUserByIdAsync(userId);
	}

	public async Task<User?> GetUserByEmailAsync(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
			throw new ArgumentException("Email is required");

		return await _repository.GetUserByEmailAsync(email);
	}

	public async Task<IEnumerable<User>> GetAllUsersAsync()
	{
		_logger.LogInformation("Fetching all users");
		return await _repository.GetAllUsersAsync();
	}

	public async Task<int> CreateUserAsync(User user)
	{
		if (string.IsNullOrWhiteSpace(user.Email))
			throw new ArgumentException("Email is required");

		if (string.IsNullOrWhiteSpace(user.Username))
			throw new ArgumentException("Username is required");

		_logger.LogInformation("Creating user '{Email}'", user.Email);
		return await _repository.AddUserAsync(user);
	}

	public async Task<bool> UpdateUserAsync(User user)
	{
		if (string.IsNullOrWhiteSpace(user.Email))
			throw new ArgumentException("Email is required");

		_logger.LogInformation("Updating user {UserId}", user.Id);
		return await _repository.UpdateUserAsync(user);
	}

	public async Task<bool> DeleteUserAsync(int userId)
	{
		_logger.LogInformation("Deleting user {UserId}", userId);
		return await _repository.DeleteUserAsync(userId);
	}
}
