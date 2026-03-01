using System.Text.Json.Nodes;
using LabEG.NeedleCrud.Models.Exceptions;
using LabEG.NeedleCrud.Repositories;
using LabEG.NeedleCrud.TestsFixtures.BLL.Entities;
using LabEG.NeedleCrud.TestsFixtures.DAL;

namespace LabEG.NeedleCrud.Tests.Repositories;

/// <summary>
/// Tests for CrudDbRepository.ExtractIncludes — validation that every graph key
/// corresponds to an actual public property of the entity type (SQL-injection guard).
/// </summary>
public class CrudDbRepositoryExtractIncludesTests : IDisposable
{
    private readonly LibraryDbContext _context;

    public CrudDbRepositoryExtractIncludesTests()
    {
        _context = LibraryDbContextFactory.Create(DatabaseProvider.InMemory, $"ExtractIncludesTestDb_{Guid.NewGuid()}");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private IList<string> ExtractIncludes(JsonObject graph)
    {
        TestableRepository repo = new(_context);
        return repo.PublicExtractIncludes(graph);
    }

    /// <summary>
    /// Thin subclass that exposes the protected ExtractIncludes for unit testing.
    /// TEntity = Book, so public properties of Book are used for validation.
    /// </summary>
    private sealed class TestableRepository : CrudDbRepository<LibraryDbContext, Book, Guid>
    {
        public TestableRepository(LibraryDbContext context) : base(context) { }

        public IList<string> PublicExtractIncludes(JsonObject graph)
        {
            return ExtractIncludes(graph);
        }

        public System.Linq.Expressions.Expression? PublicGetMemberExpression(
            string nestedProperty,
            System.Linq.Expressions.ParameterExpression param,
            Type entityType)
        {
            return GetMemberExpression(nestedProperty, param, entityType);
        }
    }

    // -------------------------------------------------------------------------
    // Valid property names — properties that really exist on Book
    // -------------------------------------------------------------------------

    [Fact]
    public void ExtractIncludes_ExistingNavigationProperty_ReturnsPath()
    {
        // Book.Author exists
        JsonObject graph = JsonNode.Parse("{\"author\":null}")!.AsObject();

        IList<string> result = ExtractIncludes(graph);

        Assert.Single(result);
        Assert.Equal("Author", result[0]);
    }

    [Fact]
    public void ExtractIncludes_MultipleExistingNavigationProperties_ReturnsAllPaths()
    {
        // Book.Author and Book.Category both exist
        JsonObject graph = JsonNode.Parse("{\"author\":null,\"category\":null}")!.AsObject();

        IList<string> result = ExtractIncludes(graph);

        Assert.Equal(2, result.Count);
        Assert.Contains("Author", result);
        Assert.Contains("Category", result);
    }

    [Fact]
    public void ExtractIncludes_NestedExistingProperty_ReturnsDotSeparatedPath()
    {
        // Book.Author → Author.Country — both levels exist
        JsonObject graph = JsonNode.Parse("{\"author\":{\"country\":null}}")!.AsObject();

        IList<string> result = ExtractIncludes(graph);

        Assert.Single(result);
        Assert.Equal("Author.Country", result[0]);
    }

    [Theory]
    [InlineData("author")]      // navigation property
    [InlineData("category")]    // navigation property
    [InlineData("loans")]       // collection navigation property
    [InlineData("reviews")]     // collection navigation property
    [InlineData("title")]       // scalar property
    [InlineData("isAvailable")] // bool scalar
    [InlineData("pageCount")]   // int scalar
    public void ExtractIncludes_AnyPublicPropertyOfBook_DoesNotThrow(string key)
    {
        JsonObject graph = JsonNode.Parse($"{{{{\"{key}\":null}}}}")!.AsObject();

        // No exception expected — key maps to a real public property of Book
        IList<string> result = ExtractIncludes(graph);
        Assert.Single(result);
    }

    // -------------------------------------------------------------------------
    // Invalid property names — properties that do NOT exist on Book
    // -------------------------------------------------------------------------

    [Fact]
    public void ExtractIncludes_NonExistentProperty_ThrowsNeedleCrudException()
    {
        JsonObject graph = JsonNode.Parse("{\"nonExistentProperty\":null}")!.AsObject();

        NeedleCrudException ex = Assert.Throws<NeedleCrudException>(
            () => ExtractIncludes(graph)
        );

        Assert.Contains("NonExistentProperty", ex.Message);
        Assert.Contains("Book", ex.Message);
    }

    [Fact]
    public void ExtractIncludes_SqlInjectionAsPropertyName_ThrowsNeedleCrudException()
    {
        // A typical SQL injection string will never match any real property name
        JsonObject graph = JsonNode.Parse("{\"author; DROP TABLE Books--\":null}")!.AsObject();

        Assert.Throws<NeedleCrudException>(
            () => ExtractIncludes(graph)
        );
    }

    [Theory]
    [InlineData("author; DROP TABLE Books--")]
    [InlineData("author' OR '1'='1")]
    [InlineData("UNION SELECT * FROM Books")]
    [InlineData("nonExistentProp")]
    [InlineData("__proto__")]
    [InlineData("bookTitle")]   // close but not exact — Title, not bookTitle
    [InlineData("authorName")]  // does not exist; Author does, AuthorName doesn't
    public void ExtractIncludes_StringNotMatchingAnyPublicProperty_ThrowsNeedleCrudException(string key)
    {
        JsonObject graph = JsonNode.Parse($"{{{{\"{key}\":null}}}}")!.AsObject();

        Assert.Throws<NeedleCrudException>(
            () => ExtractIncludes(graph)
        );
    }

    [Fact]
    public void ExtractIncludes_ValidOuterKeyButNonExistentNestedKey_ThrowsNeedleCrudException()
    {
        // author exists on Book, but "injectedField" does not exist on Author
        JsonObject graph = JsonNode.Parse("{\"author\":{\"injectedField; DROP TABLE Authors--\":null}}")!.AsObject();

        Assert.Throws<NeedleCrudException>(
            () => ExtractIncludes(graph)
        );
    }

    [Fact]
    public void ExtractIncludes_NonExistentOuterKeyWithValidNestedKey_ThrowsOnOuterKey()
    {
        // outer key does not exist on Book — should throw before even looking at nested keys
        JsonObject graph = JsonNode.Parse("{\"nonExistent\":{\"firstName\":null}}")!.AsObject();

        Assert.Throws<NeedleCrudException>(
            () => ExtractIncludes(graph)
        );
    }

    [Fact]
    public void ExtractIncludes_MixedValidAndInvalidKeys_ThrowsOnInvalidKey()
    {
        // "author" is valid, the second key is not a property of Book
        JsonObject graph = JsonNode.Parse("{\"author\":null,\"nonExistentProperty\":null}")!.AsObject();

        Assert.Throws<NeedleCrudException>(
            () => ExtractIncludes(graph)
        );
    }

    // -------------------------------------------------------------------------
    // GetMemberExpression validation tests
    // -------------------------------------------------------------------------

    [Fact]
    public void GetMemberExpression_ExistingProperty_ReturnsExpression()
    {
        // Book.Title exists
        TestableRepository repo = new(_context);
        System.Linq.Expressions.ParameterExpression param =
            System.Linq.Expressions.Expression.Parameter(typeof(Book), "b");

        System.Linq.Expressions.Expression? result =
            repo.PublicGetMemberExpression("title", param, typeof(Book));

        Assert.NotNull(result);
    }

    [Fact]
    public void GetMemberExpression_NonExistentProperty_ThrowsNeedleCrudException()
    {
        TestableRepository repo = new(_context);
        System.Linq.Expressions.ParameterExpression param =
            System.Linq.Expressions.Expression.Parameter(typeof(Book), "b");

        NeedleCrudException ex = Assert.Throws<NeedleCrudException>(
            () => repo.PublicGetMemberExpression("nonExistentProp", param, typeof(Book))
        );

        Assert.Contains("NonExistentProp", ex.Message);
        Assert.Contains("Book", ex.Message);
    }

    [Fact]
    public void GetMemberExpression_SqlInjectionProperty_ThrowsNeedleCrudException()
    {
        TestableRepository repo = new(_context);
        System.Linq.Expressions.ParameterExpression param =
            System.Linq.Expressions.Expression.Parameter(typeof(Book), "b");

        Assert.Throws<NeedleCrudException>(
            () => repo.PublicGetMemberExpression("title; DROP TABLE Books--", param, typeof(Book))
        );
    }

    [Fact]
    public void GetMemberExpression_ValidNestedProperty_ReturnsExpression()
    {
        // Book.Author.FirstName — both levels exist
        TestableRepository repo = new(_context);
        System.Linq.Expressions.ParameterExpression param =
            System.Linq.Expressions.Expression.Parameter(typeof(Book), "b");

        System.Linq.Expressions.Expression? result =
            repo.PublicGetMemberExpression("author.firstName", param, typeof(Book));

        Assert.NotNull(result);
    }

    [Fact]
    public void GetMemberExpression_InvalidNestedProperty_ThrowsNeedleCrudException()
    {
        // Book.Author exists, but Author.nonExistent does not
        TestableRepository repo = new(_context);
        System.Linq.Expressions.ParameterExpression param =
            System.Linq.Expressions.Expression.Parameter(typeof(Book), "b");

        Assert.Throws<NeedleCrudException>(
            () => repo.PublicGetMemberExpression("author.nonExistent", param, typeof(Book))
        );
    }
}
