using System.Text.Json;
using BenchmarkDotNet.Attributes;
using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.ViewModels;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using LabEG.NeedleCrud.Repositories;
using LabEG.NeedleCrud.Settings;
using LabEG.NeedleCrud.TestsFixtures.BLL.Entities;
using LabEG.NeedleCrud.TestsFixtures.DAL;
using LabEG.NeedleCrud.TestsFixtures.Fixtures;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LabEG.NeedleCrud.Benchmarks.Benchmarks;

/// <summary>
/// Benchmarks comparing performance of CrudDbRepository GetPaged method
/// against direct DbContext implementation for different query complexity levels
/// </summary>
[MemoryDiagnoser]
[SimpleJob(warmupCount: 1, iterationCount: 5)]
public class GetPagedPerformanceComparisonBenchmarks
{
    private LibraryDbContext _context = null!;
    private CrudDbRepository<LibraryDbContext, Book, Guid> _repository = null!;
    private readonly NeedleCrudSettings _settings = new();
    private PagedListQuery _simpleQuery = null!;
    private PagedListQuery _mediumQuery = null!;
    private PagedListQuery _complexQuery = null!;
    private readonly string _graphJson = """
        {
            "author": {
                "books": {
                    "category": {}
                }
            },
            "category": {
                "books": {}
            },
            "reviews": {
                "user": {}
            }
        }
        """;

    [Params(DatabaseProvider.InMemory, DatabaseProvider.PostgreSQL)]
    public DatabaseProvider Provider { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        // Create unique database name for this run
        string databaseName = $"GetPagedComparisonBenchmarkDb_{Provider}_{Guid.NewGuid():N}";

        // Create database context and schema once
        _context = LibraryDbContextFactory.Create(Provider, databaseName);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        // Seed database with test data
        TestDataGenerator.SeedDatabase(_context, seed: 42);

        // Clear change tracker
        _context.ChangeTracker.Clear();

        // Create repository
        _repository = new(_context);

        // Save changes to ensure data is persisted
        _context.SaveChanges();

        // Simple case: 1 filter, 1 sort, no graph
        _simpleQuery = new(
            pageSize: 10,
            pageNumber: 1,
            filter: "title~like~Book",
            sort: "title~asc",
            graph: null,
            settings: _settings
        );

        // Medium case: 3 filters, 3 sorts, 3 levels of nesting
        _mediumQuery = new(
            pageSize: 10,
            pageNumber: 1,
            filter: "title~like~Book,pageCount~>=~100,language~=~en",
            sort: "title~asc,pageCount~desc,publicationDate~desc",
            graph: _graphJson,
            settings: _settings
        );

        // Complex case: 5 filters, 5 sorts, 5 levels of nesting
        _complexQuery = new(
            pageSize: 10,
            pageNumber: 1,
            filter: "title~like~Book,pageCount~>=~100,language~=~en,isAvailable~=~true",
            sort: "title~asc,pageCount~desc,publicationDate~desc",
            graph: _graphJson,
            settings: _settings
        );
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Benchmark]
    public async Task<PagedList<Book>> SimpleCase_RepositoryMethod()
    {
        return await _repository.GetPaged(_simpleQuery);
    }

    [Benchmark]
    public async Task<PagedList<Book>> SimpleCase_DirectDbContext()
    {
        var queryable = _context.Books
            .Where(b => b.Title.Contains("Book"))
            .OrderBy(b => b.Title);

        var total = await queryable.CountAsync();
        var items = await queryable
            .Take(10)
            .ToListAsync();

        return new PagedList<Book>
        {
            Elements = items,
            PageMeta = new PageMeta
            {
                PageNumber = 1,
                PageSize = 10,
                TotalElements = total,
                TotalPages = (long)Math.Ceiling(total / 10.0),
                ElementsInPage = items.Count
            }
        };
    }

    [Benchmark]
    public async Task<PagedList<Book>> MediumCase_RepositoryMethod()
    {
        return await _repository.GetPaged(_mediumQuery);
    }

    [Benchmark]
    public async Task<PagedList<Book>> MediumCase_DirectDbContext()
    {
        var queryable = _context.Books
            .Include(b => b.Author)
                .ThenInclude(a => a.Books)
                    .ThenInclude(b => b.Category)
            .Include(b => b.Category)
                .ThenInclude(c => c.Books)
            .Include(b => b.Reviews)
                .ThenInclude(r => r.User)
            .Where(b => b.Title.Contains("Book") && b.PageCount >= 100 && b.Language == "en")
            .OrderBy(b => b.Title)
            .ThenByDescending(b => b.PageCount)
            .ThenByDescending(b => b.PublicationDate);

        var total = await queryable.CountAsync();
        var items = await queryable
            .Take(10)
            .ToListAsync();

        return new PagedList<Book>
        {
            Elements = items,
            PageMeta = new PageMeta
            {
                PageNumber = 1,
                PageSize = 10,
                TotalElements = total,
                TotalPages = (long)Math.Ceiling(total / 10.0),
                ElementsInPage = items.Count
            }
        };
    }

    [Benchmark]
    public async Task<PagedList<Book>> ComplexCase_RepositoryMethod()
    {
        return await _repository.GetPaged(_complexQuery);
    }

    [Benchmark]
    public async Task<PagedList<Book>> ComplexCase_DirectDbContext()
    {
        var queryable = _context.Books
            .Include(b => b.Author)
                .ThenInclude(a => a.Books)
                    .ThenInclude(b => b.Category)
            .Include(b => b.Category)
                .ThenInclude(c => c.Books)
            .Include(b => b.Reviews)
                .ThenInclude(r => r.User)
            .Where(b => b.Title.Contains("Book") && b.PageCount >= 100 && b.Language == "en" && b.IsAvailable)
            .OrderBy(b => b.Title)
            .ThenByDescending(b => b.PageCount)
            .ThenByDescending(b => b.PublicationDate);

        // Log the SQL query for debugging
        var sql = queryable.ToQueryString();
        Console.WriteLine($"Direct DbContext SQL: {sql}");

        var total = await queryable.CountAsync();
        var items = await queryable
            .Skip(0)
            .Take(10)
            .ToListAsync();

        return new PagedList<Book>
        {
            Elements = items,
            PageMeta = new PageMeta
            {
                PageNumber = 1,
                PageSize = 10,
                TotalElements = total,
                TotalPages = (long)Math.Ceiling(total / 10.0),
                ElementsInPage = items.Count
            }
        };
    }
}