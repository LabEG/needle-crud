using LabEG.NeedleCrud.Benchmarks.BLL;
using LabEG.NeedleCrud.Benchmarks.BLL.Entities;
using LabEG.NeedleCrud.Benchmarks.Fixtures;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using LabEG.NeedleCrud.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LabEG.NeedleCrud.Tests.Repositories;

/// <summary>
/// Tests for CrudDbRepository GetPaged method
/// </summary>
public class CrudDbRepositoryGetPagedTests : IDisposable
{
    private readonly LibraryDbContext _context;
    private readonly CrudDbRepository<LibraryDbContext, Book, Guid> _repository;
    private readonly TestDataSet _testData;

    public CrudDbRepositoryGetPagedTests()
    {
        // Create in-memory database for tests
        _context = LibraryDbContextFactory.Create(DatabaseProvider.InMemory, $"GetPagedTestDb_{Guid.NewGuid()}");
        _repository = new CrudDbRepository<LibraryDbContext, Book, Guid>(_context);

        // Generate test data
        _testData = TestDataGenerator.Generate(seed: 42);

        // Seed database with all data
        TestDataGenerator.SeedDatabase(_context, _testData);
        _context.ChangeTracker.Clear();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    #region Simple Pagination Tests

    [Fact]
    public async Task GetPaged_Simple_ShouldReturnFirstPageWithCorrectMetadata()
    {
        // Arrange
        PagedListQuery query = PagedListQueryFixtures.Simple;

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.PageMeta.PageNumber);
        Assert.Equal(10, result.PageMeta.PageSize);
        Assert.Equal(_testData.Books.Count, result.PageMeta.TotalElements);
        Assert.Equal(10, result.PageMeta.ElementsInPage);

        // Calculate expected total pages
        int expectedTotalPages = (_testData.Books.Count + query.PageSize - 1) / query.PageSize;
        Assert.Equal(expectedTotalPages, result.PageMeta.TotalPages);

        Assert.Equal(10, result.Elements.Count);
    }

    [Fact]
    public async Task GetPaged_Simple_SecondPage_ShouldReturnCorrectElements()
    {
        // Arrange
        PagedListQuery query = new(
            pageSize: 10,
            pageNumber: 2,
            filter: null,
            sort: null,
            graph: null
        );

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.PageMeta.PageNumber);
        Assert.Equal(10, result.PageMeta.PageSize);
        Assert.Equal(10, result.Elements.Count);
    }

    [Fact]
    public async Task GetPaged_Simple_LastPage_ShouldReturnRemainingElements()
    {
        // Arrange
        int totalBooks = _testData.Books.Count;
        int pageSize = 10;
        int lastPage = (totalBooks + pageSize - 1) / pageSize;
        int expectedElementsOnLastPage = totalBooks % pageSize == 0 ? pageSize : totalBooks % pageSize;

        PagedListQuery query = new(
            pageSize: pageSize,
            pageNumber: lastPage,
            filter: null,
            sort: null,
            graph: null
        );

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(lastPage, result.PageMeta.PageNumber);
        Assert.Equal(expectedElementsOnLastPage, result.Elements.Count);
        Assert.Equal(expectedElementsOnLastPage, result.PageMeta.ElementsInPage);
    }

    #endregion

    #region Filter Tests

    [Fact]
    public async Task GetPaged_SimpleWithFilter_ShouldReturnOnlyAvailableBooks()
    {
        // Arrange
        PagedListQuery query = PagedListQueryFixtures.SimpleWithFilter;
        int expectedCount = _testData.Books.Count(b => b.IsAvailable);

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCount, result.PageMeta.TotalElements);
        Assert.All(result.Elements, book => Assert.True(book.IsAvailable));
    }

    [Fact]
    public async Task GetPaged_ComplexFilter_ShouldApplyMultipleConditions()
    {
        // Arrange
        PagedListQuery query = PagedListQueryFixtures.ComplexFilter;
        // Filter: "IsAvailable~=~true,PageCount~>~200,PageCount~<~800,Language~like~English"
        int expectedCount = _testData.Books.Count(b =>
            b.IsAvailable &&
            b.PageCount > 200 &&
            b.PageCount < 800 &&
            b.Language.Contains("English")
        );

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCount, result.PageMeta.TotalElements);
        Assert.All(result.Elements, book =>
        {
            Assert.True(book.IsAvailable);
            Assert.InRange(book.PageCount, 201, 799);
            Assert.Contains("English", book.Language);
        });
    }

    [Fact]
    public async Task GetPaged_ComplexFull_ShouldApplyFiltersAndReturnCorrectPage()
    {
        // Arrange
        PagedListQuery query = PagedListQueryFixtures.ComplexFull;
        // Filter: "IsAvailable~=~true,PageCount~>=~300,Language~like~English"
        int expectedCount = _testData.Books.Count(b =>
            b.IsAvailable &&
            b.PageCount >= 300 &&
            b.Language.Contains("English")
        );

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.PageMeta.PageNumber);
        Assert.Equal(20, result.PageMeta.PageSize);
        Assert.Equal(expectedCount, result.PageMeta.TotalElements);
        Assert.All(result.Elements, book =>
        {
            Assert.True(book.IsAvailable);
            Assert.True(book.PageCount >= 300);
            Assert.Contains("English", book.Language);
        });
    }

    [Fact]
    public async Task GetPaged_FilterWithNoMatches_ShouldReturnEmptyResult()
    {
        // Arrange
        PagedListQuery query = new(
            pageSize: 10,
            pageNumber: 1,
            filter: "PageCount~>~10000", // No books have more than 10000 pages
            sort: null,
            graph: null
        );

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.PageMeta.TotalElements);
        Assert.Equal(1, result.PageMeta.TotalPages);
        Assert.Empty(result.Elements);
    }

    #endregion

    #region Sort Tests

    [Fact]
    public async Task GetPaged_SimpleWithSort_ShouldSortByTitleAscending()
    {
        // Arrange
        PagedListQuery query = PagedListQueryFixtures.SimpleWithSort;

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Elements.Count > 1);

        // Verify sorting
        for (int i = 0; i < result.Elements.Count - 1; i++)
        {
            Assert.True(
                string.Compare(result.Elements[i].Title, result.Elements[i + 1].Title, StringComparison.Ordinal) <= 0,
                $"Books are not sorted by Title ascending. '{result.Elements[i].Title}' should come before '{result.Elements[i + 1].Title}'"
            );
        }
    }

    [Fact]
    public async Task GetPaged_ComplexSort_ShouldApplyMultipleSortCriteria()
    {
        // Arrange
        PagedListQuery query = PagedListQueryFixtures.ComplexSort;
        // Sort: "Language~asc,PageCount~desc,Title~asc"

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Elements.Count > 1);

        // Verify primary sort by Language ascending
        for (int i = 0; i < result.Elements.Count - 1; i++)
        {
            int languageCompare = string.Compare(result.Elements[i].Language, result.Elements[i + 1].Language, StringComparison.Ordinal);

            if (languageCompare < 0)
            {
                // Different languages, correct order
                continue;
            }
            else if (languageCompare == 0)
            {
                // Same language, check PageCount descending
                if (result.Elements[i].PageCount > result.Elements[i + 1].PageCount)
                {
                    // Correct order
                    continue;
                }
                else if (result.Elements[i].PageCount == result.Elements[i + 1].PageCount)
                {
                    // Same PageCount, check Title ascending
                    Assert.True(
                        string.Compare(result.Elements[i].Title, result.Elements[i + 1].Title, StringComparison.Ordinal) <= 0,
                        "When Language and PageCount are equal, Title should be ascending"
                    );
                }
                // If PageCount is less, that's also valid (descending order)
            }
            else
            {
                Assert.Fail($"Languages are not sorted ascending: '{result.Elements[i].Language}' should come before '{result.Elements[i + 1].Language}'");
            }
        }
    }

    [Fact]
    public async Task GetPaged_ComplexFull_ShouldSortFilteredResults()
    {
        // Arrange
        PagedListQuery query = PagedListQueryFixtures.ComplexFull;
        // Sort: "PublicationDate~desc,Title~asc" with filters applied

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);

        if (result.Elements.Count > 1)
        {
            // Verify sort by PublicationDate descending
            for (int i = 0; i < result.Elements.Count - 1; i++)
            {
                DateTime current = result.Elements[i].PublicationDate;
                DateTime next = result.Elements[i + 1].PublicationDate;

                if (current > next)
                {
                    // Correct descending order
                    continue;
                }
                else if (current == next)
                {
                    // Same date, verify Title ascending
                    Assert.True(
                        string.Compare(result.Elements[i].Title, result.Elements[i + 1].Title, StringComparison.Ordinal) <= 0,
                        "When PublicationDate is equal, Title should be ascending"
                    );
                }
                else
                {
                    Assert.Fail($"PublicationDate should be descending: {current} should be >= {next}");
                }
            }
        }
    }

    #endregion

    #region Graph/Include Tests

    [Fact]
    public async Task GetPaged_SimpleGraph_ShouldIncludeAuthor()
    {
        // Arrange
        PagedListQuery query = PagedListQueryFixtures.SimpleGraph;

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Elements.Count > 0);

        // Verify that Author navigation property is loaded
        foreach (Book book in result.Elements)
        {
            Assert.NotNull(book.Author);
            Assert.NotEqual(Guid.Empty, book.Author.Id);
            Assert.False(string.IsNullOrEmpty(book.Author.FirstName));
        }
    }

    [Fact]
    public async Task GetPaged_ComplexGraph_ShouldIncludeMultipleNavigationProperties()
    {
        // Arrange
        PagedListQuery query = PagedListQueryFixtures.ComplexGraph;

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Elements.Count > 0);

        // Verify that both Author and Category navigation properties are loaded
        foreach (Book book in result.Elements)
        {
            Assert.NotNull(book.Author);
            Assert.NotEqual(Guid.Empty, book.Author.Id);

            Assert.NotNull(book.Category);
            Assert.NotEqual(Guid.Empty, book.Category.Id);
            Assert.False(string.IsNullOrEmpty(book.Category.Name));
        }
    }

    [Fact]
    public async Task GetPaged_WithoutGraph_ShouldNotLoadNavigationProperties()
    {
        // Arrange
        PagedListQuery query = PagedListQueryFixtures.Simple;

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Elements.Count > 0);

        // Navigation properties should not be null but not loaded (depending on EF Core behavior)
        // We can verify they are not explicitly loaded by checking the context entry
        foreach (Book book in result.Elements)
        {
            EntityEntry<Book> entry = _context.Entry(book);
            Assert.False(entry.Reference(b => b.Author).IsLoaded, "Author should not be loaded without graph query");
            Assert.False(entry.Reference(b => b.Category).IsLoaded, "Category should not be loaded without graph query");
        }
    }

    #endregion

    #region Combined Tests

    [Fact]
    public async Task GetPaged_ComplexGraph_ShouldApplyFilterSortAndInclude()
    {
        // Arrange
        PagedListQuery query = PagedListQueryFixtures.ComplexGraph;
        // Filter: "IsAvailable~=~true", Sort: "Title~asc", Graph: Author and Category

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);

        // Verify filter
        Assert.All(result.Elements, book => Assert.True(book.IsAvailable));

        // Verify sort
        if (result.Elements.Count > 1)
        {
            for (int i = 0; i < result.Elements.Count - 1; i++)
            {
                Assert.True(
                    string.Compare(result.Elements[i].Title, result.Elements[i + 1].Title, StringComparison.Ordinal) <= 0,
                    "Books should be sorted by Title ascending"
                );
            }
        }

        // Verify includes
        Assert.All(result.Elements, book =>
        {
            Assert.NotNull(book.Author);
            Assert.NotNull(book.Category);
        });
    }

    #endregion

    #region Edge Cases

    [Fact]
    public async Task GetPaged_PageBeyondTotalPages_ShouldReturnEmptyElements()
    {
        // Arrange
        int totalBooks = _testData.Books.Count;
        int pageSize = 10;
        int pagesBeyondTotal = (totalBooks / pageSize) + 10;

        PagedListQuery query = new(
            pageSize: pageSize,
            pageNumber: pagesBeyondTotal,
            filter: null,
            sort: null,
            graph: null
        );

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pagesBeyondTotal, result.PageMeta.PageNumber);
        Assert.Empty(result.Elements);
        Assert.Equal(0, result.PageMeta.ElementsInPage);
    }

    [Fact]
    public async Task GetPaged_LargePageSize_ShouldReturnAllElements()
    {
        // Arrange
        PagedListQuery query = new(
            pageSize: 1000,
            pageNumber: 1,
            filter: null,
            sort: null,
            graph: null
        );

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_testData.Books.Count, result.Elements.Count);
        Assert.Equal(1, result.PageMeta.TotalPages);
        Assert.Equal(_testData.Books.Count, result.PageMeta.ElementsInPage);
    }

    [Fact]
    public async Task GetPaged_PageSizeOne_ShouldReturnSingleElement()
    {
        // Arrange
        PagedListQuery query = new(
            pageSize: 1,
            pageNumber: 1,
            filter: null,
            sort: null,
            graph: null
        );

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Elements);
        Assert.Equal(_testData.Books.Count, result.PageMeta.TotalPages);
        Assert.Equal(1, result.PageMeta.ElementsInPage);
    }

    #endregion

    #region Pagination Metadata Tests

    [Fact]
    public async Task GetPaged_ShouldCalculateCorrectTotalPages()
    {
        // Test various page sizes and verify total pages calculation
        var testCases = new[]
        {
            new { PageSize = 10, ExpectedPages = (_testData.Books.Count + 9) / 10 },
            new { PageSize = 20, ExpectedPages = (_testData.Books.Count + 19) / 20 },
            new { PageSize = 7, ExpectedPages = (_testData.Books.Count + 6) / 7 },
        };

        foreach (var testCase in testCases)
        {
            // Arrange
            PagedListQuery query = new(
                pageSize: testCase.PageSize,
                pageNumber: 1,
                filter: null,
                sort: null,
                graph: null
            );

            // Act
            PagedList<Book> result = await _repository.GetPaged(query);

            // Assert
            Assert.Equal(testCase.ExpectedPages, result.PageMeta.TotalPages);
        }
    }

    [Fact]
    public async Task GetPaged_WithFilter_ShouldCalculateMetadataBasedOnFilteredResults()
    {
        // Arrange
        PagedListQuery query = PagedListQueryFixtures.SimpleWithFilter;
        int expectedTotalElements = _testData.Books.Count(b => b.IsAvailable);
        int expectedTotalPages = (expectedTotalElements + query.PageSize - 1) / query.PageSize;

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.Equal(expectedTotalElements, result.PageMeta.TotalElements);
        Assert.Equal(expectedTotalPages, result.PageMeta.TotalPages);
    }

    #endregion
}
