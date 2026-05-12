using DocumentManagement.Features.Data.Models;

namespace DocumentManagement.Features.Services.Interfaces;

public interface IUploadService
{
	Task<Upload> UploadFileAsync(Upload upload);
	Task<IEnumerable<Upload>> GetDocumentUploadsAsync(int documentId);
	Task<IEnumerable<Upload>> GetUserUploadsAsync(int userId);
	Task<Upload?> GetUploadAsync(int uploadId);
	Task<bool> DeleteUploadAsync(int uploadId);
}
