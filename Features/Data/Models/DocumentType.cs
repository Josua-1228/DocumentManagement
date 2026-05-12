using System.ComponentModel.DataAnnotations;

namespace DocumentManagement.Features.Data.Models;

public class DocumentType
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(10)]
    public string FileExtension { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [StringLength(50)]
    public string Icon { get; set; } = "bi-file-earmark";

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
