using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace LabEG.NeedleCrud.Tests.Models.ViewModels.PaginationViewModels;

public class PagedListQueryTests
{
    #region Constructor - Default Values Tests

    [Fact]
    public void Constructor_WithNullParameters_ShouldUseDefaultValues()
    {
        // Act
        PagedListQuery query = new(null, null, null, null, null);

        // Assert
        Assert.Equal(10, query.PageSize);
        Assert.Equal(1, query.PageNumber);
        Assert.Null(query.Filter);
        Assert.Null(query.Sort);
        Assert.Null(query.Graph);
    }

    [Theory]
    [InlineData(5, 2)]
    [InlineData(20, 1)]
    [InlineData(100, 10)]
    public void Constructor_WithValidPageSizeAndNumber_ShouldSetCorrectly(int pageSize, int pageNumber)
    {
        // Act
        PagedListQuery query = new(pageSize, pageNumber, null, null, null);

        // Assert
        Assert.Equal(pageSize, query.PageSize);
        Assert.Equal(pageNumber, query.PageNumber);
    }

    #endregion

    #region Filter Parsing - Positive Tests

    [Fact]
    public void Constructor_WithSingleFilter_ShouldParseCorrectly()
    {
        // Arrange
        const string filter = "name~=~John";

        // Act
        PagedListQuery query = new(null, null, filter, null, null);

        // Assert
        Assert.NotNull(query.Filter);
        PagedListQueryFilter filterItem = Assert.Single(query.Filter);
        Assert.Equal("Name", filterItem.Property);
        Assert.Equal(PagedListQueryFilterMethod.Equal, query.Filter[0].Method);
        Assert.Equal("John", query.Filter[0].Value);
    }

    [Fact]
    public void Constructor_WithMultipleFilters_ShouldParseAll()
    {
        // Arrange
        const string filter = "name~like~John,age~>=~18,city~=~NY";

        // Act
        PagedListQuery query = new(null, null, filter, null, null);

        // Assert
        Assert.NotNull(query.Filter);
        Assert.Equal(3, query.Filter.Count);

        Assert.Equal("Name", query.Filter[0].Property);
        Assert.Equal(PagedListQueryFilterMethod.Like, query.Filter[0].Method);
        Assert.Equal("John", query.Filter[0].Value);

        Assert.Equal("Age", query.Filter[1].Property);
        Assert.Equal(PagedListQueryFilterMethod.GreatOrEqual, query.Filter[1].Method);
        Assert.Equal("18", query.Filter[1].Value);

        Assert.Equal("City", query.Filter[2].Property);
        Assert.Equal(PagedListQueryFilterMethod.Equal, query.Filter[2].Method);
        Assert.Equal("NY", query.Filter[2].Value);
    }

    [Theory]
    [InlineData("price~<~100", PagedListQueryFilterMethod.Less)]
    [InlineData("price~<=~100", PagedListQueryFilterMethod.LessOrEqual)]
    [InlineData("price~=~100", PagedListQueryFilterMethod.Equal)]
    [InlineData("price~>=~100", PagedListQueryFilterMethod.GreatOrEqual)]
    [InlineData("price~>~100", PagedListQueryFilterMethod.Great)]
    [InlineData("name~like~test", PagedListQueryFilterMethod.Like)]
    [InlineData("name~ilike~test", PagedListQueryFilterMethod.ILike)]
    public void Constructor_WithDifferentFilterMethods_ShouldParseCorrectly(string filter, PagedListQueryFilterMethod expectedMethod)
    {
        // Act
        PagedListQuery query = new(null, null, filter, null, null);

        // Assert
        Assert.NotNull(query.Filter);
        PagedListQueryFilter filterItem = Assert.Single(query.Filter);
        Assert.Equal(expectedMethod, filterItem.Method);
    }

    [Fact]
    public void Constructor_WithUrlEncodedFilterValue_ShouldDecodeCorrectly()
    {
        // Arrange
        const string filter = "name~=~John%20Doe";

        // Act
        PagedListQuery query = new(null, null, filter, null, null);

        // Assert
        Assert.NotNull(query.Filter);
        Assert.Equal("John Doe", query.Filter[0].Value);
    }

    [Fact]
    public void Constructor_WithLowercasePropertyName_ShouldCapitalizeFirstLetter()
    {
        // Arrange
        const string filter = "userName~=~test";

        // Act
        PagedListQuery query = new(null, null, filter, null, null);

        // Assert
        Assert.Equal("UserName", query.Filter![0].Property);
    }

    #endregion

    #region Filter Parsing - Negative Tests

    [Fact]
    public void Constructor_WithUnknownFilterMethod_ShouldThrowBadHttpRequestException()
    {
        // Arrange
        const string filter = "name~unknown~John";

        // Act & Assert
        BadHttpRequestException exception = Assert.Throws<BadHttpRequestException>(() => new PagedListQuery(null, null, filter, null, null));
        Assert.Equal("Unknown filter method", exception.Message);
    }

    [Theory]
    [InlineData("name~=")]           // Missing value
    [InlineData("name")]              // Missing method and value
    [InlineData("")]                  // Empty string
    public void Constructor_WithInvalidFilterFormat_ShouldNotAddFilter(string filter)
    {
        // Act
        PagedListQuery query = new(null, null, filter, null, null);

        // Assert
        Assert.True(query.Filter == null || query.Filter.Count == 0);
    }

    [Fact]
    public void Constructor_WithEmptyFilterString_ShouldNotCreateFilters()
    {
        // Act
        PagedListQuery query = new(null, null, string.Empty, null, null);

        // Assert
        Assert.Null(query.Filter);
    }

    #endregion

    #region Sort Parsing - Positive Tests

    [Fact]
    public void Constructor_WithSingleSort_ShouldParseCorrectly()
    {
        // Arrange
        const string sort = "name~asc";

        // Act
        PagedListQuery query = new(null, null, null, sort, null);

        // Assert
        Assert.NotNull(query.Sort);
        PagedListQuerySort sortItem = Assert.Single(query.Sort);
        Assert.Equal("Name", sortItem.Property);
        Assert.Equal(PagedListQuerySortDirection.Asc, query.Sort[0].Direction);
    }

    [Fact]
    public void Constructor_WithMultipleSorts_ShouldParseAll()
    {
        // Arrange
        const string sort = "name~asc,age~desc,createdDate~asc";

        // Act
        PagedListQuery query = new(null, null, null, sort, null);

        // Assert
        Assert.NotNull(query.Sort);
        Assert.Equal(3, query.Sort.Count);

        Assert.Equal("Name", query.Sort[0].Property);
        Assert.Equal(PagedListQuerySortDirection.Asc, query.Sort[0].Direction);

        Assert.Equal("Age", query.Sort[1].Property);
        Assert.Equal(PagedListQuerySortDirection.Desc, query.Sort[1].Direction);

        Assert.Equal("CreatedDate", query.Sort[2].Property);
        Assert.Equal(PagedListQuerySortDirection.Asc, query.Sort[2].Direction);
    }

    [Theory]
    [InlineData("name~asc", PagedListQuerySortDirection.Asc)]
    [InlineData("name~desc", PagedListQuerySortDirection.Desc)]
    [InlineData("name~ASC", PagedListQuerySortDirection.Asc)]   // Case insensitive
    [InlineData("name~DESC", PagedListQuerySortDirection.Desc)] // Case insensitive
    [InlineData("name~Asc", PagedListQuerySortDirection.Asc)]
    public void Constructor_WithDifferentSortDirections_ShouldParseCorrectly(string sort, PagedListQuerySortDirection expectedDirection)
    {
        // Act
        PagedListQuery query = new(null, null, null, sort, null);

        // Assert
        Assert.NotNull(query.Sort);
        Assert.Equal(expectedDirection, query.Sort[0].Direction);
    }

    [Fact]
    public void Constructor_WithInvalidSortDirection_ShouldDefaultToDesc()
    {
        // Arrange - anything other than "asc" should default to desc
        const string sort = "name~invalid";

        // Act
        PagedListQuery query = new(null, null, null, sort, null);

        // Assert
        Assert.Equal(PagedListQuerySortDirection.Desc, query.Sort![0].Direction);
    }

    #endregion

    #region Sort Parsing - Negative Tests

    [Theory]
    [InlineData("name")]              // Missing direction
    [InlineData("")]                  // Empty string
    public void Constructor_WithInvalidSortFormat_ShouldNotAddSort(string sort)
    {
        // Act
        PagedListQuery query = new(null, null, null, sort, null);

        // Assert
        Assert.True(query.Sort == null || query.Sort.Count == 0);
    }

    [Fact]
    public void Constructor_WithEmptySortString_ShouldNotCreateSorts()
    {
        // Act
        PagedListQuery query = new(null, null, null, string.Empty, null);

        // Assert
        Assert.Null(query.Sort);
    }

    #endregion

    #region Graph Parsing - Positive Tests

    [Fact]
    public void Constructor_WithValidJsonGraph_ShouldParseCorrectly()
    {
        // Arrange
        const string graph = "{\"user\":null,\"items\":null}";

        // Act
        PagedListQuery query = new(null, null, null, null, graph);

        // Assert
        Assert.NotNull(query.Graph);
        Assert.IsType<JObject>(query.Graph);
        Assert.NotNull(query.Graph["user"]);
        Assert.NotNull(query.Graph["items"]);
    }

    [Fact]
    public void Constructor_WithNestedJsonGraph_ShouldParseCorrectly()
    {
        // Arrange
        const string graph = "{\"user\":{\"profile\":null}}";

        // Act
        PagedListQuery query = new(null, null, null, null, graph);

        // Assert
        Assert.NotNull(query.Graph);
        Assert.NotNull(query.Graph["user"]);
        Assert.NotNull(query.Graph["user"]!["profile"]);
    }

    [Fact]
    public void Constructor_WithEmptyJsonGraph_ShouldParseCorrectly()
    {
        // Arrange
        const string graph = "{}";

        // Act
        PagedListQuery query = new(null, null, null, null, graph);

        // Assert
        Assert.NotNull(query.Graph);
        Assert.Empty(query.Graph);
    }

    #endregion

    #region Graph Parsing - Negative Tests

    [Fact]
    public void Constructor_WithInvalidJsonGraph_ShouldNotThrow()
    {
        // Arrange
        const string graph = "{invalid json}";

        // Act - should not throw
        PagedListQuery query = new(null, null, null, null, graph);

        // Assert - Graph should be null
        Assert.Null(query.Graph);
    }

    [Fact]
    public void Constructor_WithNullGraph_ShouldNotParseGraph()
    {
        // Act
        PagedListQuery query = new(null, null, null, null, null);

        // Assert
        Assert.Null(query.Graph);
    }

    [Fact]
    public void Constructor_WithEmptyStringGraph_ShouldNotParseGraph()
    {
        // Act
        PagedListQuery query = new(null, null, null, null, string.Empty);

        // Assert
        Assert.Null(query.Graph);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void Constructor_WithAllParameters_ShouldParseAllCorrectly()
    {
        // Arrange
        const int pageSize = 25;
        const int pageNumber = 3;
        const string filter = "name~like~John,age~>=~18";
        const string sort = "name~asc,age~desc";
        const string graph = "{\"profile\":null}";

        // Act
        PagedListQuery query = new(pageSize, pageNumber, filter, sort, graph);

        // Assert
        Assert.Equal(pageSize, query.PageSize);
        Assert.Equal(pageNumber, query.PageNumber);

        Assert.Equal(2, query.Filter!.Count);
        Assert.Equal("Name", query.Filter[0].Property);
        Assert.Equal("Age", query.Filter[1].Property);

        Assert.Equal(2, query.Sort!.Count);
        Assert.Equal("Name", query.Sort[0].Property);
        Assert.Equal(PagedListQuerySortDirection.Asc, query.Sort[0].Direction);
        Assert.Equal(PagedListQuerySortDirection.Desc, query.Sort[1].Direction);

        Assert.NotNull(query.Graph);
        Assert.NotNull(query.Graph["profile"]);
    }

    [Fact]
    public void Constructor_WithComplexFilter_ShouldHandleSpecialCharacters()
    {
        // Arrange
        const string filter = "email~like~test%40example.com"; // @ is %40 in URL encoding

        // Act
        PagedListQuery query = new(null, null, filter, null, null);

        // Assert
        Assert.Equal("test@example.com", query.Filter![0].Value);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithPartiallyInvalidFilters_ShouldParseValidOnesOnly()
    {
        // Arrange
        const string filter = "name~=~John,invalid,age~>=~18";

        // Act
        PagedListQuery query = new(null, null, filter, null, null);

        // Assert
        Assert.Equal(2, query.Filter!.Count);
        Assert.Equal("Name", query.Filter[0].Property);
        Assert.Equal("Age", query.Filter[1].Property);
    }

    [Fact]
    public void Constructor_WithPartiallyInvalidSorts_ShouldParseValidOnesOnly()
    {
        // Arrange
        const string sort = "name~asc,invalid,age~desc";

        // Act
        PagedListQuery query = new(null, null, null, sort, null);

        // Assert
        Assert.Equal(2, query.Sort!.Count);
        Assert.Equal("Name", query.Sort[0].Property);
        Assert.Equal("Age", query.Sort[1].Property);
    }

    [Theory]
    [InlineData("name~=~")]           // Empty value
    [InlineData("name~=~   ")]        // Whitespace value
    public void Constructor_WithEmptyFilterValue_ShouldStillCreateFilter(string filter)
    {
        // Act
        PagedListQuery query = new(null, null, filter, null, null);

        // Assert
        PagedListQueryFilter filterItem = Assert.Single(query.Filter!);
        Assert.NotNull(filterItem.Value);
    }

    #endregion
}

