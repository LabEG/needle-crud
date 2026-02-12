# 🚀 NeedleCrud

[![NuGet](https://img.shields.io/nuget/v/LabEG.NeedleCrud.svg)](https://www.nuget.org/packages/LabEG.NeedleCrud/)
[![Downloads](https://img.shields.io/nuget/dt/LabEG.NeedleCrud.svg)](https://www.nuget.org/packages/LabEG.NeedleCrud/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0%20%7C%2010.0-512BD4)](https://dotnet.microsoft.com/)

**High-performance, lightweight CRUD library for ASP.NET Core** with advanced filtering, sorting, pagination, and eager loading—all through clean, intuitive URL query parameters. No GraphQL complexity, just simple REST with powerful capabilities.

> **⚡ Performance First**: Built with performance in mind, leveraging Expression Trees, Span<T>, and modern .NET optimizations. See our [benchmarks](LabEG.NeedleCrud.Benchmarks/README.md) for details.

---

## ✨ Why NeedleCrud?

### The Problem

Modern web APIs need flexible data querying, but:
- **GraphQL** adds complexity: new language, schema definitions, resolvers, N+1 queries
- **OData** is heavy and over-engineered for most use cases
- **Custom endpoints** lead to code duplication and maintenance nightmares

### The Solution

NeedleCrud gives you **GraphQL-like flexibility** with **REST simplicity**:

```http
GET /api/books/paged?filter=pageCount~>=~300,isAvailable~=~true&sort=title~asc&graph={"author":null,"category":null}
```

That's it. No schema, no resolvers, no learning curve. Just **URL parameters**.

---

## 🎯 Key Features

### 🔍 **Advanced Filtering**
Filter by any property with intuitive operators:
```
filter=price~>=~10,name~like~Harry,category~=~Fiction
```

**Supported operators**: `=`, `>`, `>=`, `<`, `<=`, `like` (case-sensitive), `ilike` (case-insensitive)

### 📊 **Multi-Field Sorting**
Sort by multiple fields with custom directions:
```
sort=publishDate~desc,title~asc,rating~desc
```

### 📄 **Smart Pagination**
Built-in pagination with metadata:
```
pageSize=20&pageNumber=1
```

Returns:
```json
{
  "items": [...],
  "meta": {
    "currentPage": 1,
    "pageSize": 20,
    "totalPages": 5,
    "totalCount": 87,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

### 🔗 **Eager Loading (Graph Queries)**
Load related entities with JSON-based graph syntax:
```
graph={"author":null,"reviews":null,"category":null}
```

Nested relations:
```
graph={"author":{"country":null},"reviews":{"user":null}}
```

### ⚡ **High Performance**
- Expression Tree compilation for filters and sorts
- `Span<T>` for zero-allocation parsing
- Optimized Entity Framework Core queries
- Benchmarked against real-world scenarios

---

## 🆚 NeedleCrud vs GraphQL

| Feature | NeedleCrud | GraphQL |
|---------|-----------|---------|
| **Learning Curve** | ✅ Minutes (just URL params) | ❌ Hours (schema, resolvers, queries) |
| **Setup Complexity** | ✅ 3 lines of code | ❌ Schema definitions, type system |
| **Performance** | ✅ Optimized EF Core queries | ⚠️ N+1 query problems |
| **Flexibility** | ✅ Filter, sort, paginate, eager load | ✅ Custom queries |
| **Type Safety** | ✅ C# entities | ✅ GraphQL schema |
| **Caching** | ✅ Standard HTTP caching | ⚠️ Complex caching strategies |
| **REST Compatibility** | ✅ Pure REST | ❌ Single POST endpoint |
| **Developer Tools** | ✅ Swagger, standard tools | ✅ GraphiQL, specialized tools |

**When to use NeedleCrud**: You need flexible queries without GraphQL complexity
**When to use GraphQL**: You need custom aggregations or complex computed fields

---

## 🚀 Quick Start

### Installation

```bash
dotnet add package LabEG.NeedleCrud
```

### Setup (3 Steps)

**1. Register services**
```csharp
builder.Services.AddScoped(typeof(ICrudDbRepository<,,>), typeof(CrudDbRepository<,,>));
builder.Services.AddScoped(typeof(ICrudDbService<,,>), typeof(CrudDbService<,,>));
```

**2. Add exception handler middleware**
```csharp
app.UseNeedleCrudExceptionHandler();
```

**3. Create a controller**
```csharp
[Route("api/books")]
public class BooksController(ICrudDbService<MyDbContext, Book, Guid> service)
    : CrudController<Book, Guid>(service)
{
}
```

**That's it!** You now have a full CRUD API with filtering, sorting, pagination, and eager loading.

---

## 📖 Usage Examples

### Basic CRUD Operations

```http
# Create
POST /api/books
Content-Type: application/json

{
  "title": "1984",
  "author": "George Orwell",
  "pageCount": 328
}

# Get all
GET /api/books

# Get by ID
GET /api/books/123e4567-e89b-12d3-a456-426614174000

# Update
PUT /api/books/123e4567-e89b-12d3-a456-426614174000
Content-Type: application/json

{
  "title": "Nineteen Eighty-Four",
  "author": "George Orwell",
  "pageCount": 328
}

# Delete
DELETE /api/books/123e4567-e89b-12d3-a456-426614174000
```

### Advanced Queries

**Filter books by multiple criteria:**
```http
GET /api/books/paged?filter=pageCount~>=~300,isAvailable~=~true,language~=~English
```

**Search by partial title (case-insensitive):**
```http
GET /api/books/paged?filter=title~ilike~Harry
```

**Sort by multiple fields:**
```http
GET /api/books/paged?sort=publishDate~desc,rating~desc,title~asc
```

**Paginate results:**
```http
GET /api/books/paged?pageSize=20&pageNumber=1
```

**Load related entities:**
```http
GET /api/books/paged?graph={"author":null,"category":null,"reviews":null}
```

**Combine everything:**
```http
GET /api/books/paged?
  pageSize=20&
  pageNumber=1&
  filter=pageCount~>=~300,isAvailable~=~true&
  sort=publishDate~desc,title~asc&
  graph={"author":{"country":null},"reviews":{"user":null}}
```

**Get single entity with relations:**
```http
GET /api/books/123e4567-e89b-12d3-a456-426614174000/graph?graph={"author":null,"reviews":null}
```

---

## 🎮 Try It Now!

We provide a **ready-to-run Docker container** with a sample library API:

```bash
docker pull labeg/needlecrud-sample:latest
docker run -p 8080:8080 labeg/needlecrud-sample:latest
```

Then open http://localhost:8080 to explore the **interactive Swagger UI** with:
- 📚 Books with authors, categories, reviews
- 👥 Users and loan management
- 🏷️ Categories and relationships
- All CRUD operations with live examples

**Try these sample queries:**

```http
# Get books with more than 200 pages, sorted by title
http://localhost:8080/api/books/paged?filter=pageCount~>=~200&sort=title~asc

# Search for authors by country
http://localhost:8080/api/authors/paged?filter=country~=~USA

# Get books with author and category info
http://localhost:8080/api/books/paged?graph={"author":null,"category":null}
```

---

## 📚 Documentation

### Filter Operators

| Operator | Description | Example |
|----------|-------------|---------|
| `=` | Equals | `status~=~Active` |
| `>` | Greater than | `price~>~100` |
| `>=` | Greater or equal | `rating~>=~4.5` |
| `<` | Less than | `stock~<~10` |
| `<=` | Less or equal | `age~<=~18` |
| `like` | Contains (case-sensitive) | `name~like~John` |
| `ilike` | Contains (case-insensitive) | `email~ilike~gmail` |

### Query Parameter Format

**Filter**: `property~operator~value[,property~operator~value...]`
```
filter=name~like~John,age~>=~18,city~=~London
```

**Sort**: `property~direction[,property~direction...]`
```
sort=name~asc,createdDate~desc
```

**Graph**: JSON object with navigation properties
```json
{
  "author": null,
  "category": {
    "parentCategory": null
  },
  "reviews": {
    "user": null
  }
}
```

URL-encoded:
```
graph=%7B%22author%22%3Anull%7D
```

---

## 🏗️ Architecture

NeedleCrud follows a clean layered architecture:

```
┌─────────────────────────────────────┐
│     CrudController<TEntity, TId>    │  ← REST API Layer
└─────────────────┬───────────────────┘
                  │
┌─────────────────▼───────────────────┐
│   CrudDbService<TContext, T, TId>   │  ← Business Logic Layer
└─────────────────┬───────────────────┘
                  │
┌─────────────────▼───────────────────┐
│ CrudDbRepository<TContext, T, TId>  │  ← Data Access Layer
└─────────────────┬───────────────────┘
                  │
┌─────────────────▼───────────────────┐
│      Entity Framework Core          │  ← ORM
└─────────────────────────────────────┘
```

**Key Components:**

- **Controllers**: RESTful endpoints with Swagger documentation
- **Services**: Business logic and validation
- **Repositories**: Query building with Expression Trees
- **Models**: Pagination, filtering, sorting view models

---

## ⚙️ Advanced Configuration

### Custom Controller

Override methods to add custom logic:

```csharp
public class BooksController : CrudController<Book, Guid>
{
    public BooksController(ICrudDbService<LibraryDbContext, Book, Guid> service)
        : base(service) { }

    public override async Task<Book> Create([FromBody] Book entity)
    {
        // Custom validation
        if (entity.PageCount <= 0)
            throw new ValidationException("Page count must be positive");

        // Call base implementation
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

### Custom Service Logic

```csharp
public class BookService : CrudDbService<LibraryDbContext, Book, Guid>
{
    public BookService(ICrudDbRepository<LibraryDbContext, Book, Guid> repository)
        : base(repository) { }

    public override async Task<Book> Create(Book entity)
    {
        // Custom business logic
        entity.CreatedDate = DateTime.UtcNow;
        entity.ISBN = GenerateISBN();

        return await base.Create(entity);
    }
}
```

### Entity Configuration

Entities must implement `IEntity<TId>`:

```csharp
public class Book : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public bool IsAvailable { get; set; }

    // Navigation properties
    public Guid AuthorId { get; set; }
    public Author? Author { get; set; }

    public ICollection<Review>? Reviews { get; set; }
}
```

---

## 🔧 Performance Tips

1. **Use pagination** - Always use `/paged` endpoint for large datasets
2. **Limit graph depth** - Deep nested includes can impact performance
3. **Index filtered columns** - Add database indexes for frequently filtered properties
4. **Use appropriate page sizes** - Balance between requests and payload size (20-50 items recommended)
5. **Enable response compression** - Gzip/Brotli for large payloads

### Benchmarks

See our [detailed benchmarks](LabEG.NeedleCrud.Benchmarks/README.md) for performance metrics on:
- CRUD operations (InMemory vs PostgreSQL)
- Complex filtering and sorting
- Pagination with different page sizes
- Eager loading performance

---

## 🛠️ Project Structure

```
NeedleCrud/
├── LabEG.NeedleCrud/              # Main library (NuGet package)
├── LabEG.NeedleCrud.Sample/       # Example API with Docker support
├── LabEG.NeedleCrud.Tests/        # Unit and integration tests
├── LabEG.NeedleCrud.TestsFixtures/# Shared test utilities
└── LabEG.NeedleCrud.Benchmarks/   # Performance benchmarks
```

See individual README files for detailed documentation:
- [LabEG.NeedleCrud](LabEG.NeedleCrud/README.md) - Library documentation
- [LabEG.NeedleCrud.Sample](LabEG.NeedleCrud.Sample/README.md) - Sample project guide
- [LabEG.NeedleCrud.Benchmarks](LabEG.NeedleCrud.Benchmarks/README.md) - Benchmark results

---

## 🤝 Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 💬 Support

- 📖 [Documentation](https://github.com/LabEG/needle-crud)
- 🐛 [Issue Tracker](https://github.com/LabEG/needle-crud/issues)
- 💡 [Discussions](https://github.com/LabEG/needle-crud/discussions)

---

## 🌟 Show Your Support

If NeedleCrud helps your project, give it a ⭐️ on GitHub!

---

**Made with ❤️ by LabEG**
