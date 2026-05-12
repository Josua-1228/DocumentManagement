using System.ComponentModel.DataAnnotations;

namespace DocumentManagement.Features.Data.Models;

public class Document
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [StringLength(255)]
    public string FileName { get; set; } = string.Empty;

    [StringLength(255)]
    public string FilePath { get; set; } = string.Empty;

    [StringLength(100)]
    public string ContentType { get; set; } = string.Empty;

    [StringLength(50)]
    public string Icon { get; set; } = "bi-file-earmark-text";

    public long FileSize { get; set; } = 0;

    public DateTime UploadedAt { get; set; }

    public bool IsActive { get; set; } = true;

    // Foreign keys
    public int UserId { get; set; }
    public int DocumentTypeId { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual DocumentType DocumentType { get; set; } = null!;
    public virtual ICollection<Upload> Uploads { get; set; } = new List<Upload>();
}
