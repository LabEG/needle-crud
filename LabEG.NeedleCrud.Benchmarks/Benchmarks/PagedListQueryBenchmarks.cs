using BenchmarkDotNet.Attributes;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;

namespace LabEG.NeedleCrud.Benchmarks.Benchmarks;

/// <summary>
/// Benchmarks for PagedListQuery parsing performance
/// </summary>
[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 5)]
public class PagedListQueryBenchmarks
{
    private const string SimpleFilter = "name~=~John";
    private const string ComplexFilter = "name~like~John,age~>=~18,city~=~NY,price~<~100,status~=~Active";
    private const string SimpleSort = "name~asc";
    private const string ComplexSort = "name~asc,age~desc,createdDate~asc,price~desc";
    private const string SimpleGraph = "{\"user\":null}";
    private const string ComplexGraph = "{\"user\":{\"profile\":null,\"settings\":null},\"items\":{\"category\":null}}";
    private const string UrlEncodedFilter = "email~like~test%40example.com,name~=~John%20Doe";

    [Benchmark]
    public PagedListQuery ParseSimpleFilter()
    {
        return new PagedListQuery(null, null, SimpleFilter, null, null);
    }

    [Benchmark]
    public PagedListQuery ParseComplexFilter()
    {
        return new PagedListQuery(null, null, ComplexFilter, null, null);
    }

    [Benchmark]
    public PagedListQuery ParseSimpleSort()
    {
        return new PagedListQuery(null, null, null, SimpleSort, null);
    }

    [Benchmark]
    public PagedListQuery ParseComplexSort()
    {
        return new PagedListQuery(null, null, null, ComplexSort, null);
    }

    [Benchmark]
    public PagedListQuery ParseSimpleGraph()
    {
        return new PagedListQuery(null, null, null, null, SimpleGraph);
    }

    [Benchmark]
    public PagedListQuery ParseComplexGraph()
    {
        return new PagedListQuery(null, null, null, null, ComplexGraph);
    }

    [Benchmark]
    public PagedListQuery ParseUrlEncodedFilter()
    {
        return new PagedListQuery(null, null, UrlEncodedFilter, null, null);
    }

    [Benchmark]
    public PagedListQuery ParseAllParameters()
    {
        return new PagedListQuery(25, 3, ComplexFilter, ComplexSort, ComplexGraph);
    }

    [Benchmark(Baseline = true)]
    public PagedListQuery ParseMinimal()
    {
        return new PagedListQuery(null, null, null, null, null);
    }
}
