using DocumentManagement.Features.Data.Models;
using DocumentManagement.Features.Repositories.Interfaces;
using DocumentManagement.Features.Services.Interfaces;

namespace DocumentManagement.Features.Services.Implementations;

public class UploadService : IUploadService
{
	private readonly IUploadRepository _repository;
	private readonly ILogger<UploadService> _logger;
	private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

	public UploadService(IUploadRepository repository, ILogger<UploadService> logger)
	{
		_repository = repository;
		_logger = logger;
	}

	public async Task<Upload> UploadFileAsync(Upload upload)
	{
		if (upload.FileSize > MaxFileSize)
			throw new InvalidOperationException($"File size exceeds maximum allowed size of {MaxFileSize / 1024 / 1024} MB");

		if (string.IsNullOrWhiteSpace(upload.FileName))
			throw new ArgumentException("File name is required");

		if (upload.DocumentId <= 0)
			throw new ArgumentException("Valid document ID is required");

		if (upload.UserId <= 0)
			throw new ArgumentException("Valid user ID is required");

		_logger.LogInformation("Uploading file '{FileName}' for document {DocumentId}", upload.FileName, upload.DocumentId);
		return await _repository.AddUploadAsync(upload);
	}

	public async Task<IEnumerable<Upload>> GetDocumentUploadsAsync(int documentId)
	{
		_logger.LogInformation("Fetching uploads for document {DocumentId}", documentId);
		return await _repository.GetUploadsByDocumentIdAsync(documentId);
	}

	public async Task<IEnumerable<Upload>> GetUserUploadsAsync(int userId)
	{
		_logger.LogInformation("Fetching uploads for user {UserId}", userId);
		return await _repository.GetUploadsByUserIdAsync(userId);
	}

	public async Task<Upload?> GetUploadAsync(int uploadId)
	{
		return await _repository.GetUploadByIdAsync(uploadId);
	}

	public async Task<bool> DeleteUploadAsync(int uploadId)
	{
		_logger.LogInformation("Deleting upload {UploadId}", uploadId);
		return await _repository.DeleteUploadAsync(uploadId);
	}
}
