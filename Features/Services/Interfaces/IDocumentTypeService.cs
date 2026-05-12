using DocumentManagement.Features.Data.Models;

namespace DocumentManagement.Features.Services.Interfaces;

public interface IDocumentTypeService
{
	Task<DocumentType?> GetDocumentTypeAsync(int id);
	Task<IEnumerable<DocumentType>> GetAllDocumentTypesAsync();
	Task<int> CreateDocumentTypeAsync(DocumentType documentType);
	Task<bool> UpdateDocumentTypeAsync(DocumentType documentType);
	Task<bool> DeleteDocumentTypeAsync(int id);
}
