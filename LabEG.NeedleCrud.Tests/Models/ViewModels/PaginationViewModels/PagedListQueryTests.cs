using FluentAssertions;
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
        var query = new PagedListQuery(null, null, null, null, null);

        // Assert
        query.PageSize.Should().Be(10);
        query.PageNumber.Should().Be(1);
        query.Filter.Should().BeNull();
        query.Sort.Should().BeNull();
        query.Graph.Should().BeNull();
    }

    [Theory]
    [InlineData(5, 2)]
    [InlineData(20, 1)]
    [InlineData(100, 10)]
    public void Constructor_WithValidPageSizeAndNumber_ShouldSetCorrectly(int pageSize, int pageNumber)
    {
        // Act
        var query = new PagedListQuery(pageSize, pageNumber, null, null, null);

        // Assert
        query.PageSize.Should().Be(pageSize);
        query.PageNumber.Should().Be(pageNumber);
    }

    #endregion

    #region Filter Parsing - Positive Tests

    [Fact]
    public void Constructor_WithSingleFilter_ShouldParseCorrectly()
    {
        // Arrange
        const string filter = "name~=~John";

        // Act
        var query = new PagedListQuery(null, null, filter, null, null);

        // Assert
        query.Filter.Should().NotBeNull();
        query.Filter.Should().HaveCount(1);
        query.Filter![0].Property.Should().Be("Name");
        query.Filter[0].Method.Should().Be(PagedListQueryFilterMethod.Equal);
        query.Filter[0].Value.Should().Be("John");
    }

    [Fact]
    public void Constructor_WithMultipleFilters_ShouldParseAll()
    {
        // Arrange
        const string filter = "name~like~John,age~>=~18,city~=~NY";

        // Act
        var query = new PagedListQuery(null, null, filter, null, null);

        // Assert
        query.Filter.Should().NotBeNull();
        query.Filter.Should().HaveCount(3);

        query.Filter![0].Property.Should().Be("Name");
        query.Filter[0].Method.Should().Be(PagedListQueryFilterMethod.Like);
        query.Filter[0].Value.Should().Be("John");

        query.Filter[1].Property.Should().Be("Age");
        query.Filter[1].Method.Should().Be(PagedListQueryFilterMethod.GreatOrEqual);
        query.Filter[1].Value.Should().Be("18");

        query.Filter[2].Property.Should().Be("City");
        query.Filter[2].Method.Should().Be(PagedListQueryFilterMethod.Equal);
        query.Filter[2].Value.Should().Be("NY");
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
        var query = new PagedListQuery(null, null, filter, null, null);

        // Assert
        query.Filter.Should().NotBeNull();
        query.Filter.Should().HaveCount(1);
        query.Filter![0].Method.Should().Be(expectedMethod);
    }

    [Fact]
    public void Constructor_WithUrlEncodedFilterValue_ShouldDecodeCorrectly()
    {
        // Arrange
        const string filter = "name~=~John%20Doe";

        // Act
        var query = new PagedListQuery(null, null, filter, null, null);

        // Assert
        query.Filter.Should().NotBeNull();
        query.Filter![0].Value.Should().Be("John Doe");
    }

    [Fact]
    public void Constructor_WithLowercasePropertyName_ShouldCapitalizeFirstLetter()
    {
        // Arrange
        const string filter = "userName~=~test";

        // Act
        var query = new PagedListQuery(null, null, filter, null, null);

        // Assert
        query.Filter![0].Property.Should().Be("UserName");
    }

    #endregion

    #region Filter Parsing - Negative Tests

    [Fact]
    public void Constructor_WithUnknownFilterMethod_ShouldThrowBadHttpRequestException()
    {
        // Arrange
        const string filter = "name~unknown~John";

        // Act
        Action act = () => new PagedListQuery(null, null, filter, null, null);

        // Assert
        act.Should().Throw<BadHttpRequestException>()
            .WithMessage("Unknown filter method");
    }

    [Theory]
    [InlineData("name~=")]           // Missing value
    [InlineData("name")]              // Missing method and value
    [InlineData("")]                  // Empty string
    public void Constructor_WithInvalidFilterFormat_ShouldNotAddFilter(string filter)
    {
        // Act
        var query = new PagedListQuery(null, null, filter, null, null);

        // Assert
        query.Filter.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Constructor_WithEmptyFilterString_ShouldNotCreateFilters()
    {
        // Act
        var query = new PagedListQuery(null, null, string.Empty, null, null);

        // Assert
        query.Filter.Should().BeNull();
    }

    #endregion

    #region Sort Parsing - Positive Tests

    [Fact]
    public void Constructor_WithSingleSort_ShouldParseCorrectly()
    {
        // Arrange
        const string sort = "name~asc";

        // Act
        var query = new PagedListQuery(null, null, null, sort, null);

        // Assert
        query.Sort.Should().NotBeNull();
        query.Sort.Should().HaveCount(1);
        query.Sort![0].Property.Should().Be("Name");
        query.Sort[0].Direction.Should().Be(PagedListQuerySortDirection.Asc);
    }

    [Fact]
    public void Constructor_WithMultipleSorts_ShouldParseAll()
    {
        // Arrange
        const string sort = "name~asc,age~desc,createdDate~asc";

        // Act
        var query = new PagedListQuery(null, null, null, sort, null);

        // Assert
        query.Sort.Should().NotBeNull();
        query.Sort.Should().HaveCount(3);

        query.Sort![0].Property.Should().Be("Name");
        query.Sort[0].Direction.Should().Be(PagedListQuerySortDirection.Asc);

        query.Sort[1].Property.Should().Be("Age");
        query.Sort[1].Direction.Should().Be(PagedListQuerySortDirection.Desc);

        query.Sort[2].Property.Should().Be("CreatedDate");
        query.Sort[2].Direction.Should().Be(PagedListQuerySortDirection.Asc);
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
        var query = new PagedListQuery(null, null, null, sort, null);

        // Assert
        query.Sort.Should().NotBeNull();
        query.Sort![0].Direction.Should().Be(expectedDirection);
    }

    [Fact]
    public void Constructor_WithInvalidSortDirection_ShouldDefaultToDesc()
    {
        // Arrange - anything other than "asc" should default to desc
        const string sort = "name~invalid";

        // Act
        var query = new PagedListQuery(null, null, null, sort, null);

        // Assert
        query.Sort![0].Direction.Should().Be(PagedListQuerySortDirection.Desc);
    }

    #endregion

    #region Sort Parsing - Negative Tests

    [Theory]
    [InlineData("name")]              // Missing direction
    [InlineData("")]                  // Empty string
    public void Constructor_WithInvalidSortFormat_ShouldNotAddSort(string sort)
    {
        // Act
        var query = new PagedListQuery(null, null, null, sort, null);

        // Assert
        query.Sort.Should().BeNullOrEmpty();
    }

    [Fact]
    public void Constructor_WithEmptySortString_ShouldNotCreateSorts()
    {
        // Act
        var query = new PagedListQuery(null, null, null, string.Empty, null);

        // Assert
        query.Sort.Should().BeNull();
    }

    #endregion

    #region Graph Parsing - Positive Tests

    [Fact]
    public void Constructor_WithValidJsonGraph_ShouldParseCorrectly()
    {
        // Arrange
        const string graph = "{\"user\":null,\"items\":null}";

        // Act
        var query = new PagedListQuery(null, null, null, null, graph);

        // Assert
        query.Graph.Should().NotBeNull();
        query.Graph.Should().BeOfType<JObject>();
        query.Graph!["user"].Should().NotBeNull();
        query.Graph["items"].Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNestedJsonGraph_ShouldParseCorrectly()
    {
        // Arrange
        const string graph = "{\"user\":{\"profile\":null}}";

        // Act
        var query = new PagedListQuery(null, null, null, null, graph);

        // Assert
        query.Graph.Should().NotBeNull();
        query.Graph!["user"].Should().NotBeNull();
        query.Graph["user"]!["profile"].Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithEmptyJsonGraph_ShouldParseCorrectly()
    {
        // Arrange
        const string graph = "{}";

        // Act
        var query = new PagedListQuery(null, null, null, null, graph);

        // Assert
        query.Graph.Should().NotBeNull();
        query.Graph.Should().BeEmpty();
    }

    #endregion

    #region Graph Parsing - Negative Tests

    [Fact]
    public void Constructor_WithInvalidJsonGraph_ShouldNotThrow()
    {
        // Arrange
        const string graph = "{invalid json}";

        // Act
        Action act = () => new PagedListQuery(null, null, null, null, graph);

        // Assert - should not throw, Graph should be null
        act.Should().NotThrow();
        var query = new PagedListQuery(null, null, null, null, graph);
        query.Graph.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithNullGraph_ShouldNotParseGraph()
    {
        // Act
        var query = new PagedListQuery(null, null, null, null, null);

        // Assert
        query.Graph.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithEmptyStringGraph_ShouldNotParseGraph()
    {
        // Act
        var query = new PagedListQuery(null, null, null, null, string.Empty);

        // Assert
        query.Graph.Should().BeNull();
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
        var query = new PagedListQuery(pageSize, pageNumber, filter, sort, graph);

        // Assert
        query.PageSize.Should().Be(pageSize);
        query.PageNumber.Should().Be(pageNumber);

        query.Filter.Should().HaveCount(2);
        query.Filter![0].Property.Should().Be("Name");
        query.Filter[1].Property.Should().Be("Age");

        query.Sort.Should().HaveCount(2);
        query.Sort![0].Property.Should().Be("Name");
        query.Sort[0].Direction.Should().Be(PagedListQuerySortDirection.Asc);
        query.Sort[1].Direction.Should().Be(PagedListQuerySortDirection.Desc);

        query.Graph.Should().NotBeNull();
        query.Graph!["profile"].Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithComplexFilter_ShouldHandleSpecialCharacters()
    {
        // Arrange
        const string filter = "email~like~test%40example.com"; // @ is %40 in URL encoding

        // Act
        var query = new PagedListQuery(null, null, filter, null, null);

        // Assert
        query.Filter![0].Value.Should().Be("test@example.com");
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Constructor_WithPartiallyInvalidFilters_ShouldParseValidOnesOnly()
    {
        // Arrange
        const string filter = "name~=~John,invalid,age~>=~18";

        // Act
        var query = new PagedListQuery(null, null, filter, null, null);

        // Assert
        query.Filter.Should().HaveCount(2);
        query.Filter![0].Property.Should().Be("Name");
        query.Filter[1].Property.Should().Be("Age");
    }

    [Fact]
    public void Constructor_WithPartiallyInvalidSorts_ShouldParseValidOnesOnly()
    {
        // Arrange
        const string sort = "name~asc,invalid,age~desc";

        // Act
        var query = new PagedListQuery(null, null, null, sort, null);

        // Assert
        query.Sort.Should().HaveCount(2);
        query.Sort![0].Property.Should().Be("Name");
        query.Sort[1].Property.Should().Be("Age");
    }

    [Theory]
    [InlineData("name~=~")]           // Empty value
    [InlineData("name~=~   ")]        // Whitespace value
    public void Constructor_WithEmptyFilterValue_ShouldStillCreateFilter(string filter)
    {
        // Act
        var query = new PagedListQuery(null, null, filter, null, null);

        // Assert
        query.Filter.Should().HaveCount(1);
        query.Filter![0].Value.Should().NotBeNull();
    }

    #endregion
}
