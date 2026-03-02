using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

namespace LabEG.NeedleCrud.TestsFixtures.Fixtures;

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

    // ── Navigation-property filter / sort fixtures ────────────────────────────

    /// <summary>
    /// Level-2 navigation filter — books whose author's country contains "a"
    /// (case-insensitive; nearly all country names satisfy this condition).
    /// Author must be included so the InMemory provider can evaluate the predicate.
    /// </summary>
    public static PagedListQuery FilterByNavLevel2 => new(
        pageSize: 10,
        pageNumber: 1,
        filter: "Author.Country~ilike~a",
        sort: null,
        graph: "{\"Author\":null}"
    );

    /// <summary>
    /// Level-2 navigation sort — books ordered by author's country (asc), then title (asc).
    /// Author must be included for the InMemory provider.
    /// </summary>
    public static PagedListQuery SortByNavLevel2 => new(
        pageSize: 10,
        pageNumber: 1,
        filter: null,
        sort: "Author.Country~asc,Title~asc",
        graph: "{\"Author\":null}"
    );

    /// <summary>
    /// Level-2 combined — filter by author's country containing "a",
    /// sort by author's country then category name.
    /// Both Author and Category are included.
    /// </summary>
    public static PagedListQuery FilterAndSortByNavLevel2 => new(
        pageSize: 10,
        pageNumber: 1,
        filter: "Author.Country~ilike~a",
        sort: "Author.Country~asc,Category.Name~asc",
        graph: "{\"Author\":null,\"Category\":null}"
    );

    /// <summary>
    /// Level-3 navigation filter (Review entity) — reviews for books whose author's
    /// country contains "a".  Book and Author must be included.
    /// </summary>
    public static PagedListQuery FilterByNavLevel3ForReview => new(
        pageSize: 10,
        pageNumber: 1,
        filter: "Book.Author.Country~ilike~a",
        sort: null,
        graph: "{\"Book\":{\"Author\":null}}"
    );

    /// <summary>
    /// Level-3 navigation sort (Review entity) — reviews ordered by book's author
    /// country (asc), then rating (desc).  Book and Author must be included.
    /// </summary>
    public static PagedListQuery SortByNavLevel3ForReview => new(
        pageSize: 10,
        pageNumber: 1,
        filter: null,
        sort: "Book.Author.Country~asc,Rating~desc",
        graph: "{\"Book\":{\"Author\":null}}"
    );
}
