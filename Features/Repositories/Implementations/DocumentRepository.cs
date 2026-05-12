using DocumentManagement.Features.Repositories.Interfaces;
using DocumentManagement.Features.Data;
using DocumentManagement.Features.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Features.Repositories.Implementations;

public class DocumentRepository : IDocumentRepository
{
    private readonly ApplicationDbContext _context;

    public DocumentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Document?> GetDocumentByIdAsync(int id)
    {
        return await _context.Documents
            .Include(d => d.DocumentType)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Document>> GetDocumentsByUserIdAsync(int userId)
    {
        return await _context.Documents
            .Include(d => d.DocumentType)
            .Where(d => d.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
    {
        return await _context.Documents
            .Include(d => d.DocumentType)
            .ToListAsync();
    }

    public async Task<int> AddDocumentAsync(Document document)
    {
        _context.Documents.Add(document);
        await _context.SaveChangesAsync();
        return document.Id;
    }

    public async Task<bool> UpdateDocumentAsync(Document document)
    {
        _context.Documents.Update(document);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteDocumentAsync(int id)
    {
        var document = await _context.Documents.FindAsync(id);
        if (document == null) return false;

        _context.Documents.Remove(document);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}
