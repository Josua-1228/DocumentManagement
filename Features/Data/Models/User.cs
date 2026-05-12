using System.ComponentModel.DataAnnotations;

namespace DocumentManagement.Features.Data.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(50)]
    public string Icon { get; set; } = "bi-person-circle";

    public bool IsActive { get; set; } = true;

    [StringLength(50)]
    public string Role { get; set; } = "User";

    public DateTime CreatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    // Navigation properties
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
    public virtual ICollection<Upload> Uploads { get; set; } = new List<Upload>();
}
