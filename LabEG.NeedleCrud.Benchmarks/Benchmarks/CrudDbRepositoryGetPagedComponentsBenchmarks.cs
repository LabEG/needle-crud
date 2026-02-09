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
    private string _databaseName = null!;

    private IQueryable<Book> _queryableData = null!;
    private Consumer _consumer = null!;

    [Params(DatabaseProvider.InMemory, DatabaseProvider.PostgreSQL)]
    public DatabaseProvider Provider { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        // Create unique database name for this run
        _databaseName = $"ComponentsBenchmarkDb_{Provider}_{Guid.NewGuid():N}";

        // Create database context and schema once
        _context = LibraryDbContextFactory.Create(Provider, _databaseName);
        _context.Database.EnsureCreated();

        // Seed database with test data
        TestDataGenerator.SeedDatabase(_context, seed: 42);

        // Clear change tracker
        _context.ChangeTracker.Clear();

        // Create repository with exposed methods
        _repository = new TestableRepository(_context);

        // Create consumer for IQueryable materialization
        _consumer = new Consumer();
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

    #region AddFilter Benchmarks

    [Benchmark]
    public void AddFilter_NoFilters()
    {
        IQueryable<Book> result = _repository.PublicAddFilter(_queryableData, PagedListQueryFixtures.Simple.Filter);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddFilter_SimpleWithFilter()
    {
        IQueryable<Book> result = _repository.PublicAddFilter(_queryableData, PagedListQueryFixtures.SimpleWithFilter.Filter);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddFilter_ComplexFilter()
    {
        IQueryable<Book> result = _repository.PublicAddFilter(_queryableData, PagedListQueryFixtures.ComplexFilter.Filter);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddFilter_ComplexFull()
    {
        IQueryable<Book> result = _repository.PublicAddFilter(_queryableData, PagedListQueryFixtures.ComplexFull.Filter);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddFilter_ComplexGraph()
    {
        IQueryable<Book> result = _repository.PublicAddFilter(_queryableData, PagedListQueryFixtures.ComplexGraph.Filter);
        _consumer.Consume(result.Expression);
    }

    #endregion

    #region AddSort Benchmarks

    [Benchmark]
    public void AddSort_NoSort()
    {
        IQueryable<Book> result = _repository.PublicAddSort(_queryableData, PagedListQueryFixtures.Simple.Sort);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddSort_SimpleWithSort()
    {
        IQueryable<Book> result = _repository.PublicAddSort(_queryableData, PagedListQueryFixtures.SimpleWithSort.Sort);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddSort_ComplexSort()
    {
        IQueryable<Book> result = _repository.PublicAddSort(_queryableData, PagedListQueryFixtures.ComplexSort.Sort);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddSort_ComplexFull()
    {
        IQueryable<Book> result = _repository.PublicAddSort(_queryableData, PagedListQueryFixtures.ComplexFull.Sort);
        _consumer.Consume(result.Expression);
    }

    [Benchmark]
    public void AddSort_ComplexGraph()
    {
        IQueryable<Book> result = _repository.PublicAddSort(_queryableData, PagedListQueryFixtures.ComplexGraph.Sort);
        _consumer.Consume(result.Expression);
    }

    #endregion

    #region ExtractIncludes Benchmarks

    [Benchmark]
    public IList<string> ExtractIncludes_SimpleGraph()
    {
        return _repository.PublicExtractIncludes(PagedListQueryFixtures.SimpleGraph.Graph!);
    }

    [Benchmark]
    public IList<string> ExtractIncludes_ComplexGraph()
    {
        return _repository.PublicExtractIncludes(PagedListQueryFixtures.ComplexGraph.Graph!);
    }

    #endregion

    #region GetMemberExpression Benchmarks

    [Benchmark]
    public Expression? GetMemberExpression_Simple()
    {
        ParameterExpression param = Expression.Parameter(typeof(Book), "Book");
        return _repository.PublicGetMemberExpression("Title", param, typeof(Book));
    }

    [Benchmark]
    public Expression? GetMemberExpression_Nested()
    {
        ParameterExpression param = Expression.Parameter(typeof(Book), "Book");
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
        IQueryable<Book> filtered = _repository.PublicAddFilter(_queryableData, PagedListQueryFixtures.SimpleWithFilter.Filter);
        return await filtered.CountAsync();
    }

    [Benchmark]
    public async Task<int> Combined_SimpleWithSort_SortAndCount()
    {
        IQueryable<Book> sorted = _repository.PublicAddSort(_queryableData, PagedListQueryFixtures.SimpleWithSort.Sort);
        return await sorted.CountAsync();
    }

    [Benchmark]
    public async Task<int> Combined_ComplexFilter_FilterAndCount()
    {
        IQueryable<Book> filtered = _repository.PublicAddFilter(_queryableData, PagedListQueryFixtures.ComplexFilter.Filter);
        return await filtered.CountAsync();
    }

    [Benchmark]
    public async Task<int> Combined_ComplexSort_SortAndCount()
    {
        IQueryable<Book> sorted = _repository.PublicAddSort(_queryableData, PagedListQueryFixtures.ComplexSort.Sort);
        return await sorted.CountAsync();
    }

    [Benchmark]
    public async Task<int> Combined_ComplexFull_FilterSortAndCount()
    {
        IQueryable<Book> filtered = _repository.PublicAddFilter(_queryableData, PagedListQueryFixtures.ComplexFull.Filter);
        IQueryable<Book> sorted = _repository.PublicAddSort(filtered, PagedListQueryFixtures.ComplexFull.Sort);
        return await sorted.CountAsync();
    }

    [Benchmark]
    public async Task<int> Combined_ComplexGraph_FilterSortAndCount()
    {
        IQueryable<Book> filtered = _repository.PublicAddFilter(_queryableData, PagedListQueryFixtures.ComplexGraph.Filter);
        IQueryable<Book> sorted = _repository.PublicAddSort(filtered, PagedListQueryFixtures.ComplexGraph.Sort);
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
