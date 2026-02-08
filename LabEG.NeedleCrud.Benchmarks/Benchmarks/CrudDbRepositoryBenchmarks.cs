using BenchmarkDotNet.Attributes;
using LabEG.NeedleCrud.Benchmarks.BLL;
using LabEG.NeedleCrud.Benchmarks.BLL.Entities;
using LabEG.NeedleCrud.Benchmarks.Fixtures;
using LabEG.NeedleCrud.Repositories;

namespace LabEG.NeedleCrud.Benchmarks.Benchmarks;

/// <summary>
/// Benchmarks for CrudDbRepository methods
/// </summary>
[MemoryDiagnoser]
[SimpleJob(warmupCount: 1, iterationCount: 5)]
public class CrudDbRepositoryCrudMethodsBenchmarks
{
    private LibraryDbContext _context = null!;
    private CrudDbRepository<LibraryDbContext, Book, Guid> _repository = null!;
    private TestDataSet _testData = null!;
    private Guid _existingBookId;
    private Book _bookToCreate = null!;
    private Book _bookToUpdate = null!;
    private string _databaseName = null!;

    [Params(DatabaseProvider.InMemory, DatabaseProvider.PostgreSQL)]
    public DatabaseProvider Provider { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        // Generate test data once
        _testData = TestDataGenerator.Generate(seed: 42);
        _existingBookId = _testData.Books.First().Id;

        // Create unique database name for this run
        _databaseName = $"BenchmarkDb_{Provider}_{Guid.NewGuid():N}";

        // Create database context and schema once
        _context = LibraryDbContextFactory.Create(Provider, _databaseName);
        _context.Database.EnsureCreated();

        // Create repository
        _repository = new CrudDbRepository<LibraryDbContext, Book, Guid>(_context);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        // Clear all data and reseed for each iteration
        _context.Reviews.RemoveRange(_context.Reviews);
        _context.Loans.RemoveRange(_context.Loans);
        _context.Books.RemoveRange(_context.Books);
        _context.Categories.RemoveRange(_context.Categories);
        _context.Authors.RemoveRange(_context.Authors);
        _context.Users.RemoveRange(_context.Users);
        _context.SaveChanges();

        // Clear change tracker to avoid conflicts
        _context.ChangeTracker.Clear();

        // Seed database with fresh test data
        _context.Users.AddRange(_testData.Users);
        _context.Authors.AddRange(_testData.Authors);
        _context.Categories.AddRange(_testData.Categories);
        _context.Books.AddRange(_testData.Books);
        _context.Loans.AddRange(_testData.Loans);
        _context.Reviews.AddRange(_testData.Reviews);
        _context.SaveChanges();

        // Clear change tracker again before benchmarks run
        _context.ChangeTracker.Clear();

        // Prepare entities for benchmarks
        _bookToCreate = new Book
        {
            Title = "New Benchmark Book",
            ISBN = "1234567890123",
            AuthorId = _testData.Authors.First().Id,
            CategoryId = _testData.Categories.First().Id,
            PublicationDate = DateTime.UtcNow,
            PageCount = 500,
            Publisher = "Benchmark Publisher",
            Language = "English",
            IsAvailable = true
        };

        _bookToUpdate = new Book
        {
            Id = _existingBookId,
            Title = "Updated Benchmark Book",
            ISBN = "9999999999999",
            AuthorId = _testData.Authors.Last().Id,
            CategoryId = _testData.Categories.Last().Id,
            PublicationDate = DateTime.UtcNow,
            PageCount = 600,
            Publisher = "Updated Publisher",
            Language = "Spanish",
            IsAvailable = false
        };
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        // Clear change tracker after each benchmark iteration
        _context.ChangeTracker.Clear();
    }

    [Benchmark]
    public async Task<Book> Create()
    {
        Book result = await _repository.Create(_bookToCreate);
        await _context.SaveChangesAsync();
        return result;
    }

    [Benchmark]
    public async Task<Book> GetById()
    {
        return await _repository.GetById(_existingBookId);
    }

    [Benchmark]
    public async Task<IList<Book>> GetAll()
    {
        return await _repository.GetAll();
    }

    [Benchmark]
    public async Task Update()
    {
        await _repository.Update(_existingBookId, _bookToUpdate);
        await _context.SaveChangesAsync();
    }

    [Benchmark]
    public async Task Delete()
    {
        // Use a different book for deletion each time to avoid conflicts
        Guid bookToDeleteId = _testData.Books.Skip(1).First().Id;
        await _repository.Delete(bookToDeleteId);
        await _context.SaveChangesAsync();
    }
}
