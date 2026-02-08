using System.Linq.Expressions;
using System.Text.Json.Nodes;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using LabEG.NeedleCrud.Benchmarks.BLL;
using LabEG.NeedleCrud.Benchmarks.BLL.Entities;
using LabEG.NeedleCrud.Benchmarks.Fixtures;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using LabEG.NeedleCrud.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LabEG.NeedleCrud.Benchmarks.Benchmarks;

/// <summary>
/// Benchmarks for individual components of GetPaged method
/// Uses the same query parameters as CrudDbRepositoryGetPagedBenchmarks for direct performance comparison
/// Tests AddFilter, AddSort, ExtractIncludes, GetMemberExpression, ToCamelCase, ToType
/// </summary>
[MemoryDiagnoser]
[SimpleJob(warmupCount: 1, iterationCount: 5)]
public class CrudDbRepositoryGetPagedComponentsBenchmarks
{
    private LibraryDbContext _context = null!;
    private TestableRepository _repository = null!;
    private TestDataSet _testData = null!;
    private string _databaseName = null!;

    private IQueryable<Book> _queryableData = null!;
    private Consumer _consumer = null!;

    // Same queries as in CrudDbRepositoryGetPagedBenchmarks for direct comparison
    private PagedListQuery _simpleQuery = null!;
    private PagedListQuery _simpleWithFilterQuery = null!;
    private PagedListQuery _simpleWithSortQuery = null!;
    private PagedListQuery _complexFilterQuery = null!;
    private PagedListQuery _complexSortQuery = null!;
    private PagedListQuery _complexFullQuery = null!;
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
        _databaseName = $"ComponentsBenchmarkDb_{Provider}_{Guid.NewGuid():N}";

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

        // Create repository with exposed methods
        _repository = new TestableRepository(_context);

        // Create consumer for IQueryable materialization
        _consumer = new Consumer();

        // Prepare test data
        SetupTestData();
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
        // Reset queryable data for each iteration
        _queryableData = _context.Set<Book>().AsQueryable();
        _context.ChangeTracker.Clear();
    }

    private void SetupTestData()
    {
        // Use same queries as CrudDbRepositoryGetPagedBenchmarks for direct comparison

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

    #region AddFilter Benchmarks

    [Benchmark]
    public void AddFilter_NoFilters()
    {
        var result = _repository.PublicAddFilter(_queryableData, _simpleQuery.Filter);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddFilter_SimpleWithFilter()
    {
        var result = _repository.PublicAddFilter(_queryableData, _simpleWithFilterQuery.Filter);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddFilter_ComplexFilter()
    {
        var result = _repository.PublicAddFilter(_queryableData, _complexFilterQuery.Filter);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddFilter_ComplexFull()
    {
        var result = _repository.PublicAddFilter(_queryableData, _complexFullQuery.Filter);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddFilter_ComplexGraph()
    {
        var result = _repository.PublicAddFilter(_queryableData, _complexGraphQuery.Filter);
        _consumer.Consume(result.Expression);
    }

    #endregion

    #region AddSort Benchmarks

    [Benchmark]
    public void AddSort_NoSort()
    {
        var result = _repository.PublicAddSort(_queryableData, _simpleQuery.Sort);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddSort_SimpleWithSort()
    {
        var result = _repository.PublicAddSort(_queryableData, _simpleWithSortQuery.Sort);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddSort_ComplexSort()
    {
        var result = _repository.PublicAddSort(_queryableData, _complexSortQuery.Sort);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddSort_ComplexFull()
    {
        var result = _repository.PublicAddSort(_queryableData, _complexFullQuery.Sort);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddSort_ComplexGraph()
    {
        var result = _repository.PublicAddSort(_queryableData, _complexGraphQuery.Sort);
        _consumer.Consume(result.Expression);
    }

    #endregion

    #region ExtractIncludes Benchmarks

    [Benchmark]
    public IList<string> ExtractIncludes_SimpleGraph()
    {
        return _repository.PublicExtractIncludes(_simpleGraphQuery.Graph!);
    }

    [Benchmark]
    public IList<string> ExtractIncludes_ComplexGraph()
    {
        return _repository.PublicExtractIncludes(_complexGraphQuery.Graph!);
    }

    #endregion

    #region GetMemberExpression Benchmarks

    [Benchmark]
    public Expression? GetMemberExpression_Simple()
    {
        var param = Expression.Parameter(typeof(Book), "Book");
        return _repository.PublicGetMemberExpression("Title", param, typeof(Book));
    }

    [Benchmark]
    public Expression? GetMemberExpression_Nested()
    {
        var param = Expression.Parameter(typeof(Book), "Book");
        return _repository.PublicGetMemberExpression("Author.FirstName", param, typeof(Book));
    }

    #endregion

    #region ToCamelCase Benchmarks

    [Benchmark]
    public string ToCamelCase_Short()
    {
        return _repository.PublicToCamelCase("title");
    }

    [Benchmark]
    public string ToCamelCase_Long()
    {
        return _repository.PublicToCamelCase("publicationDate");
    }

    #endregion

    #region ToType Benchmarks

    [Benchmark]
    public object? ToType_Int()
    {
        return _repository.PublicToType("500", typeof(int));
    }

    [Benchmark]
    public object? ToType_Bool()
    {
        return _repository.PublicToType("true", typeof(bool));
    }

    [Benchmark]
    public object? ToType_DateTime()
    {
        return _repository.PublicToType("2024-01-01", typeof(DateTime));
    }

    [Benchmark]
    public object? ToType_String()
    {
        return _repository.PublicToType("test value", typeof(string));
    }

    #endregion

    #region Combined Operations Benchmarks

    [Benchmark]
    public async Task<int> Combined_Simple_Count()
    {
        return await _queryableData.CountAsync();
    }

    [Benchmark]
    public async Task<int> Combined_SimpleWithFilter_FilterAndCount()
    {
        var filtered = _repository.PublicAddFilter(_queryableData, _simpleWithFilterQuery.Filter);
        return await filtered.CountAsync();
    }

    [Benchmark]
    public async Task<int> Combined_SimpleWithSort_SortAndCount()
    {
        var sorted = _repository.PublicAddSort(_queryableData, _simpleWithSortQuery.Sort);
        return await sorted.CountAsync();
    }

    [Benchmark]
    public async Task<int> Combined_ComplexFilter_FilterAndCount()
    {
        var filtered = _repository.PublicAddFilter(_queryableData, _complexFilterQuery.Filter);
        return await filtered.CountAsync();
    }

    [Benchmark]
    public async Task<int> Combined_ComplexSort_SortAndCount()
    {
        var sorted = _repository.PublicAddSort(_queryableData, _complexSortQuery.Sort);
        return await sorted.CountAsync();
    }

    [Benchmark]
    public async Task<int> Combined_ComplexFull_FilterSortAndCount()
    {
        var filtered = _repository.PublicAddFilter(_queryableData, _complexFullQuery.Filter);
        var sorted = _repository.PublicAddSort(filtered, _complexFullQuery.Sort);
        return await sorted.CountAsync();
    }

    [Benchmark]
    public async Task<int> Combined_ComplexGraph_FilterSortAndCount()
    {
        var filtered = _repository.PublicAddFilter(_queryableData, _complexGraphQuery.Filter);
        var sorted = _repository.PublicAddSort(filtered, _complexGraphQuery.Sort);
        return await sorted.CountAsync();
    }

    #endregion

    /// <summary>
    /// Testable repository that exposes protected methods for benchmarking
    /// </summary>
    private class TestableRepository : CrudDbRepository<LibraryDbContext, Book, Guid>
    {
        public TestableRepository(LibraryDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Book> PublicAddFilter(IQueryable<Book> queryableData, PagedListQueryFilter[] filters)
        {
            return AddFilter(queryableData, filters);
        }

        public IQueryable<Book> PublicAddSort(IQueryable<Book> queryableData, PagedListQuerySort[] sorts)
        {
            return AddSort(queryableData, sorts);
        }

        public IList<string> PublicExtractIncludes(JsonObject graph)
        {
            return ExtractIncludes(graph);
        }

        public Expression? PublicGetMemberExpression(string nestedProperty, ParameterExpression param, Type entityType)
        {
            return GetMemberExpression(nestedProperty, param, entityType);
        }

        public string PublicToCamelCase(string value)
        {
            return ToCamelCase(value);
        }

        public object? PublicToType(string value, Type type)
        {
            return ToType(value, type);
        }
    }
}
