using DocumentManagement.Features.Repositories.Interfaces;
using DocumentManagement.Features.Data;
using DocumentManagement.Features.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Features.Repositories.Implementations;

public class DocumentTypeRepository : IDocumentTypeRepository
{
    private readonly ApplicationDbContext _context;

    public DocumentTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DocumentType?> GetDocumentTypeByIdAsync(int id)
    {
        return await _context.DocumentTypes.FindAsync(id);
    }

    public async Task<IEnumerable<DocumentType>> GetAllDocumentTypesAsync()
    {
        return await _context.DocumentTypes.ToListAsync();
    }

    public async Task<int> AddDocumentTypeAsync(DocumentType documentType)
    {
        _context.DocumentTypes.Add(documentType);
        await _context.SaveChangesAsync();
        return documentType.Id;
    }

    public async Task<bool> UpdateDocumentTypeAsync(DocumentType documentType)
    {
        _context.DocumentTypes.Update(documentType);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteDocumentTypeAsync(int id)
    {
        var documentType = await _context.DocumentTypes.FindAsync(id);
        if (documentType == null) return false;

        _context.DocumentTypes.Remove(documentType);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}
