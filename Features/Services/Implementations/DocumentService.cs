using DocumentManagement.Features.Data.Models;
using DocumentManagement.Features.Repositories.Interfaces;
using DocumentManagement.Features.Services.Interfaces;

namespace DocumentManagement.Features.Services.Implementations;

public class DocumentService : IDocumentService
{
	private readonly IDocumentRepository _repository;
	private readonly ILogger<DocumentService> _logger;

	public DocumentService(IDocumentRepository repository, ILogger<DocumentService> logger)
	{
		_repository = repository;
		_logger = logger;
	}

	public async Task<IEnumerable<Document>> GetUserDocumentsAsync(int userId)
	{
		_logger.LogInformation("Fetching documents for user {UserId}", userId);
		return await _repository.GetDocumentsByUserIdAsync(userId);
	}

	public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
	{
		_logger.LogInformation("Fetching all documents");
		return await _repository.GetAllDocumentsAsync();
	}

	public async Task<Document?> GetDocumentAsync(int documentId)
	{
		return await _repository.GetDocumentByIdAsync(documentId);
	}

	public async Task<int> CreateDocumentAsync(Document document)
	{
		if (string.IsNullOrWhiteSpace(document.Title))
			throw new ArgumentException("Document title is required");

		if (document.UserId <= 0)
			throw new ArgumentException("Valid user ID is required");

		if (document.DocumentTypeId <= 0)
			throw new ArgumentException("Valid document type is required");

		_logger.LogInformation("Creating document '{Title}' for user {UserId}", document.Title, document.UserId);
		return await _repository.AddDocumentAsync(document);
	}

	public async Task<bool> UpdateDocumentAsync(Document document)
	{
		if (string.IsNullOrWhiteSpace(document.Title))
			throw new ArgumentException("Document title is required");

		_logger.LogInformation("Updating document {DocumentId}", document.Id);
		return await _repository.UpdateDocumentAsync(document);
	}

	public async Task<bool> DeleteDocumentAsync(int documentId)
	{
		_logger.LogInformation("Deleting document {DocumentId}", documentId);
		return await _repository.DeleteDocumentAsync(documentId);
	}
}
