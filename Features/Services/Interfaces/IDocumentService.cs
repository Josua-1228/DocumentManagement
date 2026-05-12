using DocumentManagement.Features.Data.Models;

namespace DocumentManagement.Features.Services.Interfaces;

public interface IDocumentService
{
	Task<IEnumerable<Document>> GetUserDocumentsAsync(int userId);
	Task<IEnumerable<Document>> GetAllDocumentsAsync();
	Task<Document?> GetDocumentAsync(int documentId);
	Task<int> CreateDocumentAsync(Document document);
	Task<bool> UpdateDocumentAsync(Document document);
	Task<bool> DeleteDocumentAsync(int documentId);
}
