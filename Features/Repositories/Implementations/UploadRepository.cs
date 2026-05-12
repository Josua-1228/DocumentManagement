using DocumentManagement.Features.Repositories.Interfaces;
using DocumentManagement.Features.Data;
using DocumentManagement.Features.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Features.Repositories.Implementations;

public class UploadRepository : IUploadRepository
{
    private readonly ApplicationDbContext _context;

    public UploadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Upload?> GetUploadByIdAsync(int id)
    {
        return await _context.Uploads
            .Include(u => u.Document)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<Upload>> GetUploadsByDocumentIdAsync(int documentId)
    {
        return await _context.Uploads
            .Include(u => u.Document)
            .Where(u => u.DocumentId == documentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Upload>> GetUploadsByUserIdAsync(int userId)
    {
        return await _context.Uploads
            .Include(u => u.Document)
            .Where(u => u.UserId == userId)
            .ToListAsync();
    }

    public async Task<Upload> AddUploadAsync(Upload upload)
    {
        _context.Uploads.Add(upload);
        await _context.SaveChangesAsync();
        return upload;
    }

    public async Task<bool> DeleteUploadAsync(int id)
    {
        var upload = await _context.Uploads.FindAsync(id);
        if (upload == null) return false;

        _context.Uploads.Remove(upload);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}
