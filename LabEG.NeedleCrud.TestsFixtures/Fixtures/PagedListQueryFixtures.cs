using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

namespace LabEG.NeedleCrud.Benchmarks.Fixtures;

/// <summary>
/// Provides standard PagedListQuery fixtures for benchmarks and tests
/// </summary>
public static class PagedListQueryFixtures
{
    /// <summary>
    /// Simple query - just pagination
    /// </summary>
    public static PagedListQuery Simple => new(
        pageSize: 10,
        pageNumber: 1,
        filter: null,
        sort: null,
        graph: null
    );

    /// <summary>
    /// Simple with single filter
    /// </summary>
    public static PagedListQuery SimpleWithFilter => new(
        pageSize: 10,
        pageNumber: 1,
        filter: "IsAvailable~=~true",
        sort: null,
        graph: null
    );

    /// <summary>
    /// Simple with single sort
    /// </summary>
    public static PagedListQuery SimpleWithSort => new(
        pageSize: 10,
        pageNumber: 1,
        filter: null,
        sort: "Title~asc",
        graph: null
    );

    /// <summary>
    /// Complex filter - multiple conditions
    /// </summary>
    public static PagedListQuery ComplexFilter => new(
        pageSize: 10,
        pageNumber: 1,
        filter: "IsAvailable~=~true,PageCount~>~200,PageCount~<~800,Language~like~English",
        sort: null,
        graph: null
    );

    /// <summary>
    /// Complex sort - multiple fields
    /// </summary>
    public static PagedListQuery ComplexSort => new(
        pageSize: 10,
        pageNumber: 1,
        filter: null,
        sort: "Language~asc,PageCount~desc,Title~asc",
        graph: null
    );

    /// <summary>
    /// Complex full query - filters, sorts, pagination
    /// </summary>
    public static PagedListQuery ComplexFull => new(
        pageSize: 20,
        pageNumber: 2,
        filter: "IsAvailable~=~true,PageCount~>=~300,Language~like~English",
        sort: "PublicationDate~desc,Title~asc",
        graph: null
    );

    /// <summary>
    /// Simple graph query - single include
    /// </summary>
    public static PagedListQuery SimpleGraph => new(
        pageSize: 10,
        pageNumber: 1,
        filter: null,
        sort: null,
        graph: "{\"Author\":null}"
    );

    /// <summary>
    /// Complex graph query - multiple nested includes
    /// </summary>
    public static PagedListQuery ComplexGraph => new(
        pageSize: 10,
        pageNumber: 1,
        filter: "IsAvailable~=~true",
        sort: "Title~asc",
        graph: "{\"Author\":null,\"Category\":null}"
    );
}
