# LabEG.NeedleCrud.Benchmarks

A project for measuring the performance of NeedleCrud library components using [BenchmarkDotNet](https://benchmarkdotnet.org/).

## Running Benchmarks

### Running All Benchmarks

```bash
dotnet run -c Release --framework net10.0
```

### Running Specific Benchmark

```bash
dotnet run -c Release -- --filter *PagedListQuery*
dotnet run -c Release -- --filter *CrudDbRepository*
dotnet run -c Release -- --filter *GetPaged*
dotnet run -c Release -- --filter *GetPagedComponents*
```

### Running CrudDbRepository Benchmarks

For InMemory benchmarks:
```bash
dotnet run -c Release -- --filter *CrudDbRepository* --framewor net10.0
```

For PostgreSQL benchmarks (requires a running container):
```bash
# Start PostgreSQL
docker run -d --name needlecrud-postgres \
  -e POSTGRES_DB=needlecrud-test \
  -e POSTGRES_USER=needlecrud-test \
  -e POSTGRES_PASSWORD=needlecrud-test \
  -p 5432:5432 \
  postgres:latest

# Run benchmark
dotnet run -c Release -- --filter *CrudDbRepository*

# Stop PostgreSQL
docker stop needlecrud-postgres
```

Or use the ready-made script:
```bash
# Linux/macOS
./run-crud-benchmarks.sh postgres

# Windows PowerShell
.\run-crud-benchmarks.ps1 -postgres
```

### Running with Additional Options

```bash
# Only ParseComplexFilter benchmark
dotnet run -c Release -- --filter *ParseComplexFilter

# Export results to different formats
dotnet run -c Release -- --exporters json html

# Run with specific method
dotnet run -c Release -- --filter *CrudDbRepository.Create*
```

## Available Benchmarks

### CrudDbRepositoryBenchmarks

Measures the performance of basic CRUD operations with a database:

- `Create` - creating a new entity
- `GetById` - getting an entity by ID
- `GetAll` - getting all entities
- `Update` - updating an existing entity
- `Delete` - deleting an entity

**Parameters:**
- `Provider`: InMemory, PostgreSQL

**Features:**
- Database is created once in GlobalSetup for each provider
- Data is cleared and recreated between iterations (RemoveRange + AddRange)
- ChangeTracker is cleared before and after each iteration to avoid conflicts
- Test data is generated once and used in all iterations
- Uses 100 books, 20 authors, 10 users and other related entities
- Configured SimpleJob with 1 warmup and 5 iterations for stable results

**Note:** To run the benchmark with PostgreSQL, you need to start a container:
```bash
docker run -d -it --rm --name needlecrud-postgres \
  -e POSTGRES_DB=needlecrud-test \
  -e POSTGRES_USER=needlecrud-test \
  -e POSTGRES_PASSWORD=needlecrud-test \
  -p 5432:5432 \
  postgres:latest
```

### GetPagedBenchmarks

Measures the performance of the GetPaged method on a real database with different types of queries:

- `GetPaged_Simple` - simple pagination without filters and sorting
- `GetPaged_SimpleWithFilter` - pagination with a single filter
- `GetPaged_SimpleWithSort` - pagination with single sorting
- `GetPaged_ComplexFilter` - pagination with multiple filters
- `GetPaged_ComplexSort` - pagination with multiple sorting
- `GetPaged_ComplexFull` - complex query with filters, sorting and pagination
- `GetPaged_SimpleGraph` - pagination with loading related entities (Include)
- `GetPaged_ComplexGraph` - pagination with multiple Includes

**Parameters:**
- `Provider`: InMemory, PostgreSQL

**Features:**
- Tests the complete lifecycle of the GetPaged method from start to finish
- Uses real data (100 books, 20 authors, 10 users)
- Measures performance on simple and complex queries
- Allows comparing the speed of InMemory and PostgreSQL

### GetPagedComponentsBenchmarks

Measures the performance of individual components of the GetPaged method:

**AddFilter:**
- `AddFilter_Simple` - single filter
- `AddFilter_Complex` - multiple filters with different operators
- `AddFilter_NoFilters` - baseline without filters

**AddSort:**
- `AddSort_Simple` - single sort
- `AddSort_Complex` - multiple sorting by several fields
- `AddSort_NoSort` - baseline without sorting

**ExtractIncludes:**
- `ExtractIncludes_Simple` - extracting one Include from graph
- `ExtractIncludes_Complex` - extracting multiple Includes

**GetMemberExpression:**
- `GetMemberExpression_Simple` - simple property
- `GetMemberExpression_Nested` - nested property (navigation property)

**ToCamelCase:**
- `ToCamelCase_Short` - short string
- `ToCamelCase_Long` - long string

**ToType:**
- `ToType_Int` - conversion to int
- `ToType_Bool` - conversion to bool
- `ToType_DateTime` - conversion to DateTime
- `ToType_String` - conversion to string

**Combined Operations:**
- `Combined_FilterAndCount` - filtering + counting
- `Combined_SortAndCount` - sorting + counting
- `Combined_FilterSortAndCount` - filtering + sorting + counting

**Parameters:**
- `Provider`: InMemory, PostgreSQL

**Features:**
- Measures the performance of each component separately
- Helps identify bottlenecks in query processing logic
- Uses real data for accurate measurements
- Allows optimizing individual methods independently

### PagedListQueryBenchmarks

Measures the performance of parsing query parameters:

- `ParseSimpleFilter` - parsing a single filter
- `ParseComplexFilter` - parsing multiple filters
- `ParseSimpleSort` - parsing a single sort
- `ParseComplexSort` - parsing multiple sorts
- `ParseSimpleGraph` - parsing a simple JSON graph
- `ParseComplexGraph` - parsing a nested JSON graph
- `ParseUrlEncodedFilter` - parsing URL-encoded values
- `ParseAllParameters` - parsing all parameters simultaneously
- `ParseMinimal` - baseline (without parameters)

## Results

Benchmark results are saved in the `BenchmarkDotNet.Artifacts/results/` folder.

## Adding New Benchmarks

Create a new class with the `[MemoryDiagnoser]` attribute and methods with the `[Benchmark]` attribute:

```csharp
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class MyBenchmarks
{
    [Benchmark]
    public void MyMethod()
    {
        // code to measure
    }
}
```

## Useful Attributes

- `[MemoryDiagnoser]` - shows memory allocation
- `[SimpleJob]` - configures the number of runs
- `[Benchmark(Baseline = true)]` - comparison with baseline
- `[Params(...)]` - benchmark parameterization
- `[GlobalSetup]` - executed once before all iterations
- `[IterationSetup]` - executed before each iteration
- `[IterationCleanup]` - executed after each iteration

## Documentation

- [BenchmarkDotNet Documentation](https://benchmarkdotnet.org/articles/overview.html)
- [Best Practices](https://benchmarkdotnet.org/articles/guides/good-practices.html)