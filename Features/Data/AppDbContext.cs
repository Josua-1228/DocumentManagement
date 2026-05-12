using DocumentManagement.Features.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Features.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets for all entities
    public DbSet<User> Users { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentType> DocumentTypes { get; set; }
    public DbSet<Upload> Uploads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
        });

        // Configure DocumentType entity
        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FileExtension).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        // Configure Document entity
        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.FileSize).HasDefaultValue(0);
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            // Foreign key relationships
            entity.HasOne(d => d.User)
                  .WithMany(u => u.Documents)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.DocumentType)
                  .WithMany(dt => dt.Documents)
                  .HasForeignKey(d => d.DocumentTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Upload entity
        modelBuilder.Entity<Upload>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("GETUTCDATE()");

            // Foreign key relationships
            entity.HasOne(u => u.User)
                  .WithMany(u => u.Uploads)
                  .HasForeignKey(u => u.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Document)
                  .WithMany(d => d.Uploads)
                  .HasForeignKey(u => u.DocumentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data for document types
        modelBuilder.Entity<DocumentType>().HasData(
            new DocumentType { Id = 1, Name = "PDF Document", FileExtension = ".pdf", Description = "Portable Document Format", Icon = "bi-file-earmark-pdf", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new DocumentType { Id = 2, Name = "Word Document", FileExtension = ".docx", Description = "Microsoft Word Document", Icon = "bi-file-earmark-word", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new DocumentType { Id = 3, Name = "Excel Spreadsheet", FileExtension = ".xlsx", Description = "Microsoft Excel Spreadsheet", Icon = "bi-file-earmark-excel", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new DocumentType { Id = 4, Name = "PowerPoint Presentation", FileExtension = ".pptx", Description = "Microsoft PowerPoint Presentation", Icon = "bi-file-earmark-slides", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new DocumentType { Id = 5, Name = "Text Document", FileExtension = ".txt", Description = "Plain Text Document", Icon = "bi-file-earmark-text", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) },
            new DocumentType { Id = 6, Name = "Image File", FileExtension = ".jpg,.png,.gif", Description = "Image Files (JPEG, PNG, GIF)", Icon = "bi-file-earmark-image", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) }
        );

        // Seed data for a default admin user
        modelBuilder.Entity<User>().HasData(
            new User 
            { 
                Id = 1, 
                Username = "admin", 
                Email = "admin@documentmanagement.com", 
                FullName = "System Administrator", 
                Icon = "bi-shield-check",
                IsActive = true, 
                CreatedAt = new DateTime(2024, 1, 1) 
            }
        );
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is User || e.Entity is Document || e.Entity is DocumentType || e.Entity is Upload);

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is User user && user.CreatedAt == default)
                        user.CreatedAt = DateTime.UtcNow;
                    else if (entry.Entity is Document doc && doc.UploadedAt == default)
                        doc.UploadedAt = DateTime.UtcNow;
                    else if (entry.Entity is DocumentType docType && docType.CreatedAt == default)
                        docType.CreatedAt = DateTime.UtcNow;
                    else if (entry.Entity is Upload upload && upload.UploadedAt == default)
                        upload.UploadedAt = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    // You could add updated_at fields here if needed
                    break;
            }
        }
    }
}
