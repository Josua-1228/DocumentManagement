using DocumentManagement.Features.Repositories.Interfaces;
using DocumentManagement.Features.Data.Models;

namespace DocumentManagement.Features.Repositories.Interfaces;

public interface IUploadRepository
{
	Task<Upload?> GetUploadByIdAsync(int id);
	Task<IEnumerable<Upload>> GetUploadsByDocumentIdAsync(int documentId);
	Task<IEnumerable<Upload>> GetUploadsByUserIdAsync(int userId);
	Task<Upload> AddUploadAsync(Upload upload);
	Task<bool> DeleteUploadAsync(int id);
}
