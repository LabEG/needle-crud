using LabEG.NeedleCrud.Benchmarks.BLL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LabEG.NeedleCrud.Benchmarks.BLL;

/// <summary>
/// Database provider type
/// </summary>
public enum DatabaseProvider
{
    /// <summary>
    /// In-memory database provider for testing
    /// </summary>
    InMemory,

    /// <summary>
    /// PostgreSQL database provider
    /// </summary>
    PostgreSQL
}

/// <summary>
/// Database context for the library system
/// </summary>
/// <remarks>
/// To run PostgreSQL container for testing:
/// <code>
/// docker run -d --name needlecrud-postgres \
///   -e POSTGRES_DB=needlecrud-test \
///   -e POSTGRES_USER=needlecrud-test \
///   -e POSTGRES_PASSWORD=needlecrud-test \
///   -p 5432:5432 \
///   postgres:latest
/// </code>
/// </remarks>
public class LibraryDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LibraryDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the Users DbSet
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Authors DbSet
    /// </summary>
    public DbSet<Author> Authors { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Categories DbSet
    /// </summary>
    public DbSet<Category> Categories { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Books DbSet
    /// </summary>
    public DbSet<Book> Books { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Loans DbSet
    /// </summary>
    public DbSet<Loan> Loans { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Reviews DbSet
    /// </summary>
    public DbSet<Review> Reviews { get; set; } = null!;

    /// <summary>
    /// Configures the schema needed for the library context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(500);
        });

        // Author configuration
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.Biography).HasMaxLength(2000);
        });

        // Category configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
        });

        // Book configuration
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
            entity.Property(e => e.ISBN).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Publisher).HasMaxLength(200);
            entity.Property(e => e.Language).HasMaxLength(50);

            entity.HasIndex(e => e.ISBN).IsUnique();
            entity.HasIndex(e => e.AuthorId);
            entity.HasIndex(e => e.CategoryId);
        });

        // Loan configuration
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LateFee).HasPrecision(18, 2);
            entity.Property(e => e.Notes).HasMaxLength(1000);

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.BookId);
            entity.HasIndex(e => new { e.UserId, e.BookId });
        });

        // Review configuration
        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Comment).HasMaxLength(2000);
            entity.Property(e => e.Rating).IsRequired();

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.BookId);
            entity.HasIndex(e => new { e.BookId, e.UserId });
        });
    }
}

/// <summary>
/// Factory for creating LibraryDbContext instances
/// </summary>
public static class LibraryDbContextFactory
{
    private const string PostgreSqlConnectionString =
        "Host=localhost;Database=needlecrud-test;Username=needlecrud-test;Password=needlecrud-test";

    /// <summary>
    /// Creates DbContext with specified database provider
    /// </summary>
    /// <param name="provider">Database provider type</param>
    /// <param name="databaseName">Database name (used for InMemory provider)</param>
    /// <returns>Configured LibraryDbContext instance</returns>
    public static LibraryDbContext Create(DatabaseProvider provider, string databaseName = "TestDatabase")
    {
        DbContextOptionsBuilder<LibraryDbContext> optionsBuilder = new();

        switch (provider)
        {
            case DatabaseProvider.InMemory:
                optionsBuilder.UseInMemoryDatabase(databaseName);
                break;
            case DatabaseProvider.PostgreSQL:
                optionsBuilder.UseNpgsql(PostgreSqlConnectionString);
                break;
            default:
                throw new ArgumentException($"Unsupported database provider: {provider}", nameof(provider));
        }

        return new LibraryDbContext(optionsBuilder.Options);
    }

}
