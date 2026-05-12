using System.ComponentModel.DataAnnotations;

namespace DocumentManagement.Features.Data.Models;

public class Upload
{
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string FileName { get; set; } = string.Empty;

    [StringLength(500)]
    public string FilePath { get; set; } = string.Empty;

    [StringLength(100)]
    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; }

    // Foreign keys
    public int UserId { get; set; }
    public int DocumentId { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Document Document { get; set; } = null!;
}
