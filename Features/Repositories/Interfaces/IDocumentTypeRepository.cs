using DocumentManagement.Features.Repositories.Interfaces;
using DocumentManagement.Features.Data.Models;

namespace DocumentManagement.Features.Repositories.Interfaces;

public interface IDocumentTypeRepository
{
	Task<DocumentType?> GetDocumentTypeByIdAsync(int id);
	Task<IEnumerable<DocumentType>> GetAllDocumentTypesAsync();
	Task<int> AddDocumentTypeAsync(DocumentType documentType);
	Task<bool> UpdateDocumentTypeAsync(DocumentType documentType);
	Task<bool> DeleteDocumentTypeAsync(int id);
}
