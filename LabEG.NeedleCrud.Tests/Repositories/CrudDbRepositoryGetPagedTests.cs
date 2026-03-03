using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using LabEG.NeedleCrud.Repositories;
using LabEG.NeedleCrud.Settings;
using LabEG.NeedleCrud.TestsFixtures.BLL.Entities;
using LabEG.NeedleCrud.TestsFixtures.DAL;
using LabEG.NeedleCrud.TestsFixtures.Fixtures;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LabEG.NeedleCrud.Tests.Repositories;

/// <summary>
/// Tests for CrudDbRepository GetPaged method
/// </summary>
public class CrudDbRepositoryGetPagedTests : IDisposable
{
    private readonly LibraryDbContext _context;
    private readonly CrudDbRepository<LibraryDbContext, Book, Guid> _repository;
    private readonly CrudDbRepository<LibraryDbContext, Review, Guid> _reviewRepository;
    private readonly TestDataSet _testData;

    public CrudDbRepositoryGetPagedTests()
    {
        // Create in-memory database for tests
        _context = LibraryDbContextFactory.Create(DatabaseProvider.InMemory, $"GetPagedTestDb_{Guid.NewGuid()}");
        _repository = new CrudDbRepository<LibraryDbContext, Book, Guid>(_context);
        _reviewRepository = new CrudDbRepository<LibraryDbContext, Review, Guid>(_context);

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
        Assert.Equal(1, result.Meta.PageNumber);
        Assert.Equal(10, result.Meta.PageSize);
        Assert.Equal(_testData.Books.Count, result.Meta.TotalItems);
        Assert.Equal(10, result.Meta.ItemsInPage);

        // Calculate expected total pages
        int expectedTotalPages = (_testData.Books.Count + query.PageSize - 1) / query.PageSize;
        Assert.Equal(expectedTotalPages, result.Meta.TotalPages);

        Assert.Equal(10, result.Items.Count);
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
        Assert.Equal(2, result.Meta.PageNumber);
        Assert.Equal(10, result.Meta.PageSize);
        Assert.Equal(10, result.Items.Count);
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
        Assert.Equal(lastPage, result.Meta.PageNumber);
        Assert.Equal(expectedElementsOnLastPage, result.Items.Count);
        Assert.Equal(expectedElementsOnLastPage, result.Meta.ItemsInPage);
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
        Assert.Equal(expectedCount, result.Meta.TotalItems);
        Assert.All(result.Items, book => Assert.True(book.IsAvailable));
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
        Assert.Equal(expectedCount, result.Meta.TotalItems);
        Assert.All(result.Items, book =>
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
        Assert.Equal(2, result.Meta.PageNumber);
        Assert.Equal(20, result.Meta.PageSize);
        Assert.Equal(expectedCount, result.Meta.TotalItems);
        Assert.All(result.Items, book =>
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
        Assert.Equal(0, result.Meta.TotalItems);
        Assert.Equal(1, result.Meta.TotalPages);
        Assert.Empty(result.Items);
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
        Assert.True(result.Items.Count > 1);

        // Verify sorting
        for (int i = 0; i < result.Items.Count - 1; i++)
        {
            Assert.True(
                string.Compare(result.Items[i].Title, result.Items[i + 1].Title, StringComparison.Ordinal) <= 0,
                $"Books are not sorted by Title ascending. '{result.Items[i].Title}' should come before '{result.Items[i + 1].Title}'"
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
        Assert.True(result.Items.Count > 1);

        // Verify primary sort by Language ascending
        for (int i = 0; i < result.Items.Count - 1; i++)
        {
            int languageCompare = string.Compare(result.Items[i].Language, result.Items[i + 1].Language, StringComparison.Ordinal);

            if (languageCompare < 0)
            {
                // Different languages, correct order
                continue;
            }
            else if (languageCompare == 0)
            {
                // Same language, check PageCount descending
                if (result.Items[i].PageCount > result.Items[i + 1].PageCount)
                {
                    // Correct order
                    continue;
                }
                else if (result.Items[i].PageCount == result.Items[i + 1].PageCount)
                {
                    // Same PageCount, check Title ascending
                    Assert.True(
                        string.Compare(result.Items[i].Title, result.Items[i + 1].Title, StringComparison.Ordinal) <= 0,
                        "When Language and PageCount are equal, Title should be ascending"
                    );
                }
                // If PageCount is less, that's also valid (descending order)
            }
            else
            {
                Assert.Fail($"Languages are not sorted ascending: '{result.Items[i].Language}' should come before '{result.Items[i + 1].Language}'");
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

        if (result.Items.Count > 1)
        {
            // Verify sort by PublicationDate descending
            for (int i = 0; i < result.Items.Count - 1; i++)
            {
                DateTime current = result.Items[i].PublicationDate;
                DateTime next = result.Items[i + 1].PublicationDate;

                if (current > next)
                {
                    // Correct descending order
                    continue;
                }
                else if (current == next)
                {
                    // Same date, verify Title ascending
                    Assert.True(
                        string.Compare(result.Items[i].Title, result.Items[i + 1].Title, StringComparison.Ordinal) <= 0,
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
        Assert.True(result.Items.Count > 0);

        // Verify that Author navigation property is loaded
        foreach (Book book in result.Items)
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
        Assert.True(result.Items.Count > 0);

        // Verify that both Author and Category navigation properties are loaded
        foreach (Book book in result.Items)
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
        Assert.True(result.Items.Count > 0);

        // Navigation properties should not be null but not loaded (depending on EF Core behavior)
        // We can verify they are not explicitly loaded by checking the context entry
        foreach (Book book in result.Items)
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
        Assert.All(result.Items, book => Assert.True(book.IsAvailable));

        // Verify sort
        if (result.Items.Count > 1)
        {
            for (int i = 0; i < result.Items.Count - 1; i++)
            {
                Assert.True(
                    string.Compare(result.Items[i].Title, result.Items[i + 1].Title, StringComparison.Ordinal) <= 0,
                    "Books should be sorted by Title ascending"
                );
            }
        }

        // Verify includes
        Assert.All(result.Items, book =>
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
        Assert.Equal(pagesBeyondTotal, result.Meta.PageNumber);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.Meta.ItemsInPage);
    }

    [Fact]
    public async Task GetPaged_LargePageSize_ShouldReturnAllElements()
    {
        // Arrange
        NeedleCrudSettings settings = new()
        {
            MaxPageSize = 2000 // Allow larger page size for this test
        };
        PagedListQuery query = new(
            pageSize: 1000,
            pageNumber: 1,
            filter: null,
            sort: null,
            graph: null,
            settings: settings
        );

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_testData.Books.Count, result.Items.Count);
        Assert.Equal(1, result.Meta.TotalPages);
        Assert.Equal(_testData.Books.Count, result.Meta.ItemsInPage);
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
        Assert.Single(result.Items);
        Assert.Equal(_testData.Books.Count, result.Meta.TotalPages);
        Assert.Equal(1, result.Meta.ItemsInPage);
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
            Assert.Equal(testCase.ExpectedPages, result.Meta.TotalPages);
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
        Assert.Equal(expectedTotalElements, result.Meta.TotalItems);
        Assert.Equal(expectedTotalPages, result.Meta.TotalPages);
    }

    #endregion

    #region Navigation Property Filter and Sort Tests

    [Fact]
    public async Task GetPaged_FilterByNavLevel2_AuthorCountry_ShouldReturnMatchingBooks()
    {
        // Arrange
        // Pick a country that is guaranteed to exist in generated data
        string country = _testData.Authors
            .First(a => !string.IsNullOrEmpty(a.Country)).Country;

        int expectedCount = _testData.Books.Count(b =>
        {
            Author? author = _testData.Authors.FirstOrDefault(a => a.Id == b.AuthorId);
            return author?.Country == country;
        });

        // Author must be included so the InMemory provider can evaluate the predicate
        PagedListQuery query = new(
            pageSize: 100,
            pageNumber: 1,
            filter: $"Author.Country~=~{country}",
            sort: null,
            graph: "{\"Author\":null}"
        );

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCount, result.Meta.TotalItems);
        Assert.All(result.Items, book =>
        {
            Assert.NotNull(book.Author);
            Assert.Equal(country, book.Author.Country);
        });
    }

    [Fact]
    public async Task GetPaged_FilterByNavLevel2_CategoryName_ShouldReturnMatchingBooks()
    {
        // Arrange
        string categoryName = _testData.Categories
            .First(c => !string.IsNullOrEmpty(c.Name)).Name;

        int expectedCount = _testData.Books.Count(b =>
        {
            Category? category = _testData.Categories.FirstOrDefault(c => c.Id == b.CategoryId);
            return category?.Name == categoryName;
        });

        PagedListQuery query = new(
            pageSize: 100,
            pageNumber: 1,
            filter: $"Category.Name~=~{categoryName}",
            sort: null,
            graph: "{\"Category\":null}"
        );

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCount, result.Meta.TotalItems);
        Assert.All(result.Items, book =>
        {
            Assert.NotNull(book.Category);
            Assert.Equal(categoryName, book.Category.Name);
        });
    }

    [Fact]
    public async Task GetPaged_SortByNavLevel2_AuthorCountry_ShouldOrderBooksCorrectly()
    {
        // Arrange
        PagedListQuery query = new(
            pageSize: 50,
            pageNumber: 1,
            filter: null,
            sort: "Author.Country~asc,Title~asc",
            graph: "{\"Author\":null}"
        );

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Items.Count > 1);

        for (int i = 0; i < result.Items.Count - 1; i++)
        {
            int countryCompare = string.Compare(
                result.Items[i].Author.Country,
                result.Items[i + 1].Author.Country,
                StringComparison.Ordinal);

            if (countryCompare < 0) continue; // correct ascending order

            if (countryCompare > 0)
                Assert.Fail(
                    $"Books not sorted by Author.Country asc: '{result.Items[i].Author.Country}' > '{result.Items[i + 1].Author.Country}'");

            // Same country — verify secondary sort by Title ascending
            Assert.True(
                string.Compare(result.Items[i].Title, result.Items[i + 1].Title, StringComparison.Ordinal) <= 0,
                "When Author.Country is equal, Title should be ascending");
        }
    }

    [Fact]
    public async Task GetPaged_FilterAndSortByNavLevel2_ShouldCombineCorrectly()
    {
        // Arrange — filter books whose author's country contains "a" (ilike),
        //           then sort by author's country asc, category name asc
        PagedListQuery query = new(
            pageSize: 50,
            pageNumber: 1,
            filter: "Author.Country~ilike~a",
            sort: "Author.Country~asc,Category.Name~asc",
            graph: "{\"Author\":null,\"Category\":null}"
        );

        int expectedCount = _testData.Books.Count(b =>
        {
            Author? author = _testData.Authors.FirstOrDefault(a => a.Id == b.AuthorId);
            return author?.Country.Contains('a', StringComparison.InvariantCultureIgnoreCase) == true;
        });

        // Act
        PagedList<Book> result = await _repository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCount, result.Meta.TotalItems);

        // Verify filter — every book's author country must contain 'a'
        Assert.All(result.Items, book =>
        {
            Assert.NotNull(book.Author);
            Assert.True(
                book.Author.Country.Contains('a', StringComparison.InvariantCultureIgnoreCase),
                $"Author.Country '{book.Author.Country}' should contain 'a' (case-insensitive)");
        });

        // Verify sort — author country ascending, then category name ascending
        for (int i = 0; i < result.Items.Count - 1; i++)
        {
            int countryCompare = string.Compare(
                result.Items[i].Author.Country,
                result.Items[i + 1].Author.Country,
                StringComparison.Ordinal);

            if (countryCompare < 0) continue;

            if (countryCompare > 0)
                Assert.Fail("Books not sorted by Author.Country ascending");

            // Same author country — check Category.Name secondary sort
            Assert.True(
                string.Compare(result.Items[i].Category!.Name, result.Items[i + 1].Category!.Name, StringComparison.Ordinal) <= 0,
                "When Author.Country is equal, Category.Name should be ascending");
        }
    }

    [Fact]
    public async Task GetPaged_Review_FilterByNavLevel3_BookAuthorCountry_ShouldReturnMatchingReviews()
    {
        // Arrange
        string country = _testData.Authors
            .First(a => !string.IsNullOrEmpty(a.Country)).Country;

        HashSet<Guid> bookIdsByAuthorCountry = _testData.Books
            .Where(b => _testData.Authors.Any(a => a.Id == b.AuthorId && a.Country == country))
            .Select(b => b.Id)
            .ToHashSet();

        int expectedCount = _testData.Reviews
            .Count(r => bookIdsByAuthorCountry.Contains(r.BookId));

        // Book and Author must be included so InMemory can evaluate the three-level predicate
        NeedleCrudSettings settings = new() { MaxPageSize = 300 };
        PagedListQuery query = new(
            pageSize: 300,
            pageNumber: 1,
            filter: $"Book.Author.Country~=~{country}",
            sort: null,
            graph: "{\"Book\":{\"Author\":null}}",
            settings: settings
        );

        // Act
        PagedList<Review> result = await _reviewRepository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCount, result.Meta.TotalItems);
        Assert.All(result.Items, review =>
        {
            Assert.NotNull(review.Book);
            Assert.NotNull(review.Book.Author);
            Assert.Equal(country, review.Book.Author.Country);
        });
    }

    [Fact]
    public async Task GetPaged_Review_SortByNavLevel3_BookAuthorCountry_ShouldOrderCorrectly()
    {
        // Arrange
        PagedListQuery query = new(
            pageSize: 50,
            pageNumber: 1,
            filter: null,
            sort: "Book.Author.Country~asc,Rating~desc",
            graph: "{\"Book\":{\"Author\":null}}"
        );

        // Act
        PagedList<Review> result = await _reviewRepository.GetPaged(query);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Items.Count > 1);

        for (int i = 0; i < result.Items.Count - 1; i++)
        {
            int countryCompare = string.Compare(
                result.Items[i].Book.Author.Country,
                result.Items[i + 1].Book.Author.Country,
                StringComparison.Ordinal);

            if (countryCompare < 0) continue;

            if (countryCompare > 0)
                Assert.Fail(
                    $"Reviews not sorted by Book.Author.Country asc: '{result.Items[i].Book.Author.Country}' > '{result.Items[i + 1].Book.Author.Country}'");

            // Same country — verify secondary sort by Rating descending
            Assert.True(
                result.Items[i].Rating >= result.Items[i + 1].Rating,
                "When Book.Author.Country is equal, Rating should be descending");
        }
    }

    #endregion
}
