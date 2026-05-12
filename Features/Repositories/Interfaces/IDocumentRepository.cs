using DocumentManagement.Features.Repositories.Interfaces;
using DocumentManagement.Features.Data.Models;

namespace DocumentManagement.Features.Repositories.Interfaces;

public interface IDocumentRepository
{
	Task<Document?> GetDocumentByIdAsync(int id);
	Task<IEnumerable<Document>> GetDocumentsByUserIdAsync(int userId);
	Task<IEnumerable<Document>> GetAllDocumentsAsync();
	Task<int> AddDocumentAsync(Document document);
	Task<bool> UpdateDocumentAsync(Document document);
	Task<bool> DeleteDocumentAsync(int id);
}
