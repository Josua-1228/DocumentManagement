using DocumentManagement.Features.Data.Models;
using DocumentManagement.Features.Repositories.Interfaces;
using DocumentManagement.Features.Services.Interfaces;

namespace DocumentManagement.Features.Services.Implementations;

public class DocumentTypeService : IDocumentTypeService
{
	private readonly IDocumentTypeRepository _repository;
	private readonly ILogger<DocumentTypeService> _logger;

	public DocumentTypeService(IDocumentTypeRepository repository, ILogger<DocumentTypeService> logger)
	{
		_repository = repository;
		_logger = logger;
	}

	public async Task<DocumentType?> GetDocumentTypeAsync(int id)
	{
		return await _repository.GetDocumentTypeByIdAsync(id);
	}

	public async Task<IEnumerable<DocumentType>> GetAllDocumentTypesAsync()
	{
		_logger.LogInformation("Fetching all document types");
		return await _repository.GetAllDocumentTypesAsync();
	}

	public async Task<int> CreateDocumentTypeAsync(DocumentType documentType)
	{
		if (string.IsNullOrWhiteSpace(documentType.Name))
			throw new ArgumentException("Document type name is required");

		_logger.LogInformation("Creating document type '{Name}'", documentType.Name);
		return await _repository.AddDocumentTypeAsync(documentType);
	}

	public async Task<bool> UpdateDocumentTypeAsync(DocumentType documentType)
	{
		if (string.IsNullOrWhiteSpace(documentType.Name))
			throw new ArgumentException("Document type name is required");

		_logger.LogInformation("Updating document type {DocumentTypeId}", documentType.Id);
		return await _repository.UpdateDocumentTypeAsync(documentType);
	}

	public async Task<bool> DeleteDocumentTypeAsync(int id)
	{
		_logger.LogInformation("Deleting document type {DocumentTypeId}", id);
		return await _repository.DeleteDocumentTypeAsync(id);
	}
}
