# LabEG.NeedleCrud

[![NuGet](https://img.shields.io/nuget/v/LabEG.NeedleCrud.svg)](https://www.nuget.org/packages/LabEG.NeedleCrud/)
[![Downloads](https://img.shields.io/nuget/dt/LabEG.NeedleCrud.svg)](https://www.nuget.org/packages/LabEG.NeedleCrud/)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0%20%7C%2010.0-512BD4)](https://dotnet.microsoft.com/)

High-performance CRUD library for ASP.NET Core with advanced filtering, sorting, pagination, and eager loading through URL query parameters.

## Installation

```bash
dotnet add package LabEG.NeedleCrud
```

## Quick Start

### 1. Register Services

```csharp
builder.Services.AddScoped(typeof(ICrudDbRepository<,,>), typeof(CrudDbRepository<,,>));
builder.Services.AddScoped(typeof(ICrudDbService<,,>), typeof(CrudDbService<,,>));
```

### 2. Add Middleware

```csharp
app.UseNeedleCrudExceptionHandler();
```

### 3. Create Controller

```csharp
[Route("api/books")]
public class BooksController(ICrudDbService<MyDbContext, Book, Guid> service)
    : CrudController<Book, Guid>(service)
{
}
```

## Features

- ✅ **Complete CRUD operations** - Create, Read, Update, Delete
- ✅ **Advanced filtering** - `filter=name~like~John,age~>=~18`
- ✅ **Multi-field sorting** - `sort=name~asc,date~desc`
- ✅ **Pagination with metadata** - `pageSize=20&pageNumber=1`
- ✅ **Eager loading** - `graph={"author":null,"reviews":null}`
- ✅ **High performance** - Expression Trees, Span<T>, optimized queries
- ✅ **Multi-targeting** - .NET 8.0, 9.0, 10.0

## Usage Examples

### Basic Operations

```http
# Create
POST /api/books
{
  "title": "1984",
  "pageCount": 328
}

# Get all
GET /api/books

# Get by ID
GET /api/books/{id}

# Update
PUT /api/books/{id}
{
  "title": "Nineteen Eighty-Four",
  "pageCount": 328
}

# Delete
DELETE /api/books/{id}
```

### Advanced Queries

```http
# Filtering
GET /api/books/paged?filter=pageCount~>=~300,isAvailable~=~true

# Sorting
GET /api/books/paged?sort=title~asc,publishDate~desc

# Pagination
GET /api/books/paged?pageSize=20&pageNumber=1

# Eager loading
GET /api/books/paged?graph={"author":null,"category":null}

# Combined
GET /api/books/paged?pageSize=20&filter=pageCount~>=~300&sort=title~asc&graph={"author":null}
```

## Filter Operators

| Operator | Description | Example |
|----------|-------------|---------|
| `=` | Equals | `status~=~Active` |
| `>` | Greater than | `price~>~100` |
| `>=` | Greater or equal | `rating~>=~4.5` |
| `<` | Less than | `stock~<~10` |
| `<=` | Less or equal | `age~<=~18` |
| `like` | Contains (case-sensitive) | `name~like~John` |
| `ilike` | Contains (case-insensitive) | `email~ilike~gmail` |

## Entity Requirements

Entities must implement `IEntity<TId>`:

```csharp
public class Book : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int PageCount { get; set; }

    // Navigation properties for eager loading
    public Author? Author { get; set; }
    public ICollection<Review>? Reviews { get; set; }
}
```

## Custom Logic

### Custom Controller

```csharp
public class BooksController : CrudController<Book, Guid>
{
    public BooksController(ICrudDbService<LibraryDbContext, Book, Guid> service)
        : base(service) { }

    public override async Task<Book> Create([FromBody] Book entity)
    {
        // Add custom validation
        if (entity.PageCount <= 0)
            throw new ValidationException("Invalid page count");

        return await base.Create(entity);
    }

    // Add custom endpoints
    [HttpGet("bestsellers")]
    public async Task<Book[]> GetBestsellers()
    {
        var query = new PagedListQuery(
            pageSize: 10,
            filter: "rating~>=~4.5",
            sort: "soldCopies~desc"
        );
        var result = await Service.GetPaged(query);
        return result.Items;
    }
}
```

### Custom Service

```csharp
public class BookService : CrudDbService<LibraryDbContext, Book, Guid>
{
    public BookService(ICrudDbRepository<LibraryDbContext, Book, Guid> repository)
        : base(repository) { }

    public override async Task<Book> Create(Book entity)
    {
        entity.CreatedDate = DateTime.UtcNow;
        return await base.Create(entity);
    }
}

// Register custom service
builder.Services.AddScoped<ICrudDbService<LibraryDbContext, Book, Guid>, BookService>();
```

## Architecture

```
CrudController<TEntity, TId>      ← REST API endpoints
    ↓
CrudDbService<TContext, T, TId>   ← Business logic
    ↓
CrudDbRepository<TContext, T, TId> ← Data access with EF Core
```

## Performance

- **Expression Trees** - Compiled queries for filters and sorting
- **Span<T>** - Zero-allocation string parsing
- **Optimized EF Core** - Efficient database queries
- See [benchmarks](../LabEG.NeedleCrud.Benchmarks/README.md) for detailed metrics

## Try It Out

Run the sample Docker container:

```bash
docker pull labeg/needlecrud-sample:latest
docker run -p 8080:8080 labeg/needlecrud-sample:latest
```

Open http://localhost:8080 for interactive Swagger UI.

## Documentation

- [Full Documentation](../README.md)
- [Sample Project](../LabEG.NeedleCrud.Sample/README.md)
- [Benchmarks](../LabEG.NeedleCrud.Benchmarks/README.md)

## Support

- 📖 [GitHub Repository](https://github.com/LabEG/NeedleCrud)
- 🐛 [Issue Tracker](https://github.com/LabEG/NeedleCrud/issues)
- 💡 [Discussions](https://github.com/LabEG/NeedleCrud/discussions)

## License

MIT License - see [LICENSE](../LICENSE) for details.

---

**Made with ❤️ by LabEG**
