using BenchmarkDotNet.Attributes;
using LabEG.NeedleCrud.Benchmarks.BLL;
using LabEG.NeedleCrud.Benchmarks.BLL.Entities;
using LabEG.NeedleCrud.Benchmarks.Fixtures;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using LabEG.NeedleCrud.Repositories;

namespace LabEG.NeedleCrud.Benchmarks.Benchmarks;

/// <summary>
/// Benchmarks for CrudDbRepository GetPaged method and its components
/// Tests simple and complex queries on both InMemory and PostgreSQL databases
/// </summary>
[MemoryDiagnoser]
[SimpleJob(warmupCount: 1, iterationCount: 5)]
public class CrudDbRepositoryGetPagedBenchmarks
{
    private LibraryDbContext _context = null!;
    private CrudDbRepository<LibraryDbContext, Book, Guid> _repository = null!;
    private string _databaseName = null!;

    [Params(DatabaseProvider.InMemory, DatabaseProvider.PostgreSQL)]
    public DatabaseProvider Provider { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        // Create unique database name for this run
        _databaseName = $"GetPagedBenchmarkDb_{Provider}_{Guid.NewGuid():N}";

        // Create database context and schema once
        _context = LibraryDbContextFactory.Create(Provider, _databaseName);
        _context.Database.EnsureDeleted(); // Delete first to avoid duplicates
        _context.Database.EnsureCreated();

        // Seed database with test data
        TestDataGenerator.SeedDatabase(_context, seed: 42);

        // Clear change tracker
        _context.ChangeTracker.Clear();

        // Create repository
        _repository = new CrudDbRepository<LibraryDbContext, Book, Guid>(_context);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    #region Full GetPaged Benchmarks

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_Simple()
    {
        return await _repository.GetPaged(PagedListQueryFixtures.Simple);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_SimpleWithFilter()
    {
        return await _repository.GetPaged(PagedListQueryFixtures.SimpleWithFilter);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_SimpleWithSort()
    {
        return await _repository.GetPaged(PagedListQueryFixtures.SimpleWithSort);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_ComplexFilter()
    {
        return await _repository.GetPaged(PagedListQueryFixtures.ComplexFilter);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_ComplexSort()
    {
        return await _repository.GetPaged(PagedListQueryFixtures.ComplexSort);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_ComplexFull()
    {
        return await _repository.GetPaged(PagedListQueryFixtures.ComplexFull);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_SimpleGraph()
    {
        return await _repository.GetPaged(PagedListQueryFixtures.SimpleGraph);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_ComplexGraph()
    {
        return await _repository.GetPaged(PagedListQueryFixtures.ComplexGraph);
    }

    #endregion
}
