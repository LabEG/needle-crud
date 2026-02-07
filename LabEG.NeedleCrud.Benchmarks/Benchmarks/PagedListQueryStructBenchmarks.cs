using BenchmarkDotNet.Attributes;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

namespace LabEG.NeedleCrud.Benchmarks.Benchmarks;

[MemoryDiagnoser]
public class PagedListQueryStructBenchmarks
{
    private const string SimpleFilter = "name~like~John";
    private const string ComplexFilter = "firstName~ilike~John%20Doe%20%26%20Smith";
    private const string SimpleSort = "name~asc";
    private const string ComplexSort = "lastModifiedDateTime~desc";

    [Benchmark]
    public PagedListQueryFilter CreateSimpleFilter()
    {
        return new PagedListQueryFilter(SimpleFilter.AsSpan());
    }

    [Benchmark]
    public PagedListQueryFilter CreateComplexFilter()
    {
        return new PagedListQueryFilter(ComplexFilter.AsSpan());
    }

    [Benchmark]
    public PagedListQuerySort CreateSimpleSort()
    {
        return new PagedListQuerySort(SimpleSort.AsSpan());
    }

    [Benchmark]
    public PagedListQuerySort CreateComplexSort()
    {
        return new PagedListQuerySort(ComplexSort.AsSpan());
    }

    [Benchmark]
    public string AccessFilterProperties()
    {
        var filter = new PagedListQueryFilter(ComplexFilter.AsSpan());
        return filter.Property + filter.Value + filter.Method.ToString();
    }

    [Benchmark]
    public string AccessSortProperties()
    {
        var sort = new PagedListQuerySort(ComplexSort.AsSpan());
        return sort.Property + sort.Direction.ToString();
    }

    [Benchmark]
    public PagedListQueryFilter[] CreateMultipleFilters()
    {
        var filters = new PagedListQueryFilter[5];
        filters[0] = new PagedListQueryFilter("id~=~123".AsSpan());
        filters[1] = new PagedListQueryFilter("name~like~John".AsSpan());
        filters[2] = new PagedListQueryFilter("age~>=~18".AsSpan());
        filters[3] = new PagedListQueryFilter("status~=~active".AsSpan());
        filters[4] = new PagedListQueryFilter("date~<~2024-01-01".AsSpan());
        return filters;
    }

    [Benchmark]
    public PagedListQuerySort[] CreateMultipleSorts()
    {
        var sorts = new PagedListQuerySort[3];
        sorts[0] = new PagedListQuerySort("name~asc".AsSpan());
        sorts[1] = new PagedListQuerySort("date~desc".AsSpan());
        sorts[2] = new PagedListQuerySort("priority~asc".AsSpan());
        return sorts;
    }
}
