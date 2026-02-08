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
    private TestDataSet _testData = null!;
    private string _databaseName = null!;

    // Simple queries
    private PagedListQuery _simpleQuery = null!;
    private PagedListQuery _simpleWithFilterQuery = null!;
    private PagedListQuery _simpleWithSortQuery = null!;

    // Complex queries
    private PagedListQuery _complexFilterQuery = null!;
    private PagedListQuery _complexSortQuery = null!;
    private PagedListQuery _complexFullQuery = null!;

    // Graph queries
    private PagedListQuery _simpleGraphQuery = null!;
    private PagedListQuery _complexGraphQuery = null!;

    [Params(DatabaseProvider.InMemory, DatabaseProvider.PostgreSQL)]
    public DatabaseProvider Provider { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        // Generate test data once
        _testData = TestDataGenerator.Generate(seed: 42);

        // Create unique database name for this run
        _databaseName = $"GetPagedBenchmarkDb_{Provider}_{Guid.NewGuid():N}";

        // Create database context and schema once
        _context = LibraryDbContextFactory.Create(Provider, _databaseName);
        _context.Database.EnsureCreated();

        // Seed database with test data
        _context.Users.AddRange(_testData.Users);
        _context.Authors.AddRange(_testData.Authors);
        _context.Categories.AddRange(_testData.Categories);
        _context.Books.AddRange(_testData.Books);
        _context.Loans.AddRange(_testData.Loans);
        _context.Reviews.AddRange(_testData.Reviews);
        _context.SaveChanges();

        // Clear change tracker
        _context.ChangeTracker.Clear();

        // Create repository
        _repository = new CrudDbRepository<LibraryDbContext, Book, Guid>(_context);

        // Prepare queries
        SetupQueries();
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private void SetupQueries()
    {
        // Simple query - just pagination
        _simpleQuery = new PagedListQuery(
            pageSize: 10,
            pageNumber: 1,
            filter: null,
            sort: null,
            graph: null
        );

        // Simple with single filter
        _simpleWithFilterQuery = new PagedListQuery(
            pageSize: 10,
            pageNumber: 1,
            filter: "IsAvailable~=~true",
            sort: null,
            graph: null
        );

        // Simple with single sort
        _simpleWithSortQuery = new PagedListQuery(
            pageSize: 10,
            pageNumber: 1,
            filter: null,
            sort: "Title~asc",
            graph: null
        );

        // Complex filter - multiple conditions
        _complexFilterQuery = new PagedListQuery(
            pageSize: 10,
            pageNumber: 1,
            filter: "IsAvailable~=~true,PageCount~>~200,PageCount~<~800,Language~like~English",
            sort: null,
            graph: null
        );

        // Complex sort - multiple fields
        _complexSortQuery = new PagedListQuery(
            pageSize: 10,
            pageNumber: 1,
            filter: null,
            sort: "Language~asc,PageCount~desc,Title~asc",
            graph: null
        );

        // Complex full query - filters, sorts, pagination
        _complexFullQuery = new PagedListQuery(
            pageSize: 20,
            pageNumber: 2,
            filter: "IsAvailable~=~true,PageCount~>=~300,Language~like~English",
            sort: "PublicationDate~desc,Title~asc",
            graph: null
        );

        // Simple graph query - single include
        _simpleGraphQuery = new PagedListQuery(
            pageSize: 10,
            pageNumber: 1,
            filter: null,
            sort: null,
            graph: "{\"Author\":null}"
        );

        // Complex graph query - multiple nested includes
        _complexGraphQuery = new PagedListQuery(
            pageSize: 10,
            pageNumber: 1,
            filter: "IsAvailable~=~true",
            sort: "Title~asc",
            graph: "{\"Author\":null,\"Category\":null}"
        );
    }

    #region Full GetPaged Benchmarks

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_Simple()
    {
        return await _repository.GetPaged(_simpleQuery);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_SimpleWithFilter()
    {
        return await _repository.GetPaged(_simpleWithFilterQuery);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_SimpleWithSort()
    {
        return await _repository.GetPaged(_simpleWithSortQuery);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_ComplexFilter()
    {
        return await _repository.GetPaged(_complexFilterQuery);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_ComplexSort()
    {
        return await _repository.GetPaged(_complexSortQuery);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_ComplexFull()
    {
        return await _repository.GetPaged(_complexFullQuery);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_SimpleGraph()
    {
        return await _repository.GetPaged(_simpleGraphQuery);
    }

    [Benchmark]
    public async Task<PagedList<Book>> GetPaged_ComplexGraph()
    {
        return await _repository.GetPaged(_complexGraphQuery);
    }

    #endregion
}
