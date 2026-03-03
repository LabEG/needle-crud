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

**Filter by navigation property (level 2) — books written by a specific author:**
```http
GET /api/books/paged?filter=author.name~ilike~Tolkien
```

**Filter by navigation property (level 2) — books in a specific category:**
```http
GET /api/books/paged?filter=category.name~=~Science Fiction
```

**Filter by navigation property (level 3) — reviews for books from a specific country's author:**
```http
GET /api/reviews/paged?filter=book.author.country~=~UK
```

**Filter by navigation property (level 3) — loans for books in a given category:**
```http
GET /api/loans/paged?filter=book.category.name~=~Fiction&graph={"book":{"author":null}}
```

**Sort by multiple fields:**
```http
GET /api/books/paged?sort=publishDate~desc,rating~desc,title~asc
```

**Sort by navigation property (level 2) — by author's name:**
```http
GET /api/books/paged?sort=author.name~asc
```

**Sort by navigation property (level 2) — by category name, then by title:**
```http
GET /api/books/paged?sort=category.name~asc,title~asc
```

**Sort by navigation property (level 3) — reviews sorted by the book's author country:**
```http
GET /api/reviews/paged?sort=book.author.country~asc,book.title~asc
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

Navigation properties use dot notation — any depth is supported:
```
filter=author.country~=~UK                          # level 2
filter=book.author.name~ilike~Orwell                # level 3
filter=loan.book.category.name~=~Fiction            # level 4
```

**Sort**: `property~direction[,property~direction...]`
```
sort=name~asc,createdDate~desc
```

Navigation properties use dot notation — any depth is supported:
```
sort=author.name~asc                          # level 2
sort=book.author.country~asc,title~asc        # level 3
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

## 🏛️ Entity Design Guide

NeedleCrud, like GraphQL, exposes your **entity model directly as the API surface**. Every public property becomes filterable and sortable; every navigation property becomes a graph node for eager loading. This means entity design decisions have a direct and immediate impact on API behaviour, security, and performance.

### Why it matters

With a hand-written REST controller you can shape the response in the action method. With NeedleCrud (or GraphQL) the entity *is* the contract — so you must design it deliberately.

### Rules

#### 1. Implement `IEntity<TId>` and use a simple primary key

```csharp
// ✅ Good — Guid or int, never a composite key
public class Book : IEntity<Guid>
{
    public Guid Id { get; set; }
    // ...
}
```

#### 2. Never expose sensitive data as public properties

Every public property is reachable via `filter=`, `sort=`, and the JSON response. Sensitive fields (passwords, tokens, internal flags) must be excluded.

```csharp
// ❌ Bad — PasswordHash will appear in API responses and be filterable
public class User : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // exposed!
}

// ✅ Good — keep secrets out of entity or use [JsonIgnore]
public class User : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;

    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;
}
```

#### 3. Declare explicit foreign key properties

EF Core works with shadow FK properties, but NeedleCrud filtering needs a named, navigable property path. Declare both the FK scalar and the navigation property:

```csharp
// ✅ Good
public class Book : IEntity<Guid>
{
    public Guid Id { get; set; }

    public Guid AuthorId { get; set; }     // scalar FK — filterable: author.id
    public Author? Author { get; set; }    // navigation — graph-loadable
}
```

#### 4. Make navigation properties nullable and use `[JsonIgnore]` on inverse navigations

Navigation properties that point *back* to the parent create circular reference cycles during JSON serialisation. Mark inverse navigations with `[JsonIgnore]`:

```csharp
// ✅ Good
public class Author : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]                               // prevents circular reference
    public ICollection<Book> Books { get; set; } = [];
}

public class Book : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public Author? Author { get; set; }        // nullable — loaded on demand
}
```

#### 5. Use scalar types for properties that will be filtered or sorted

Filtering and sorting work on scalar values (`string`, `int`, `bool`, `DateTime`, `Guid`, `decimal`, …). Navigation properties themselves cannot be used as filter targets — only their nested scalar fields can.

```csharp
// ✅ filter=author.country~=~UK   ← scalar field on a navigation property
// ❌ filter=author~=~something    ← navigation object — not supported
```

#### 6. Keep the graph shallow — avoid deep or wide navigation trees

Every level of eager loading adds a `JOIN`. Design entities so that the data you really need is reachable in 2–3 levels. Move rarely-needed aggregates to dedicated endpoints.

```
Book → Author (level 2) → Country (level 3) ← reasonable
Book → Author → Books → Reviews → User → … ← avoid
```

#### 7. Use `ICollection<T>` (not `List<T>`) for collection navigations

EF Core and System.Text.Json serialise both, but `ICollection<T>` is the conventional choice and avoids accidental coupling to `List<T>` methods.

#### 8. Initialize collection navigations to avoid null reference exceptions

```csharp
public ICollection<Review> Reviews { get; set; } = [];
```

#### 9. Consider a DTO projection layer for complex read scenarios

When the entity shape needed by clients diverges significantly from the storage model, introduce explicit ViewModels and override the `GetPaged` / `GetById` methods in your controller to project with `Select(...)`.

### Further reading

| Topic | Resource |
|-------|----------|
| EF Core data model conventions | [EF Core – Creating and Configuring a Model](https://learn.microsoft.com/en-us/ef/core/modeling/) |
| EF Core relationships | [EF Core – Relationships](https://learn.microsoft.com/en-us/ef/core/modeling/relationships) |
| EF Core performance | [EF Core – Performance](https://learn.microsoft.com/en-us/ef/core/performance/) |
| Avoiding circular references in JSON | [System.Text.Json – How to handle circular references](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/preserve-references) |
| REST API design | [Microsoft Azure – API design best practices](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design) |
| GraphQL schema design (conceptual parallel) | [GraphQL – Best Practices](https://graphql.org/learn/best-practices/) |

---

## 🔒 Security

NeedleCrud exposes all CRUD operations automatically, which is powerful — and requires you to think about access control from the start. Below are the recommended patterns.

### Rate Limiting

Protect your API from abuse by adding a fixed-window rate limiter (built into .NET 7+). No extra NuGet packages are needed.

> **Note:** Rate limiting is **not** part of NeedleCrud itself. You add it at the application level using standard ASP.NET Core middleware, which is the correct approach — this keeps the library small and gives you full control over the policy.

```csharp
// Program.cs — register rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100;            // max 100 requests per minute per client
        opt.QueueLimit = 0;
    });

    options.OnRejected = async (context, ct) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Too many requests. Try again later.", ct);
    };
});

// ...

// Program.cs — add to pipeline
app.UseRateLimiter();
```

Apply the policy to all NeedleCrud controllers at once by adding an `[EnableRateLimiting]` attribute to a custom base controller, or globally via a `IApplicationBuilder` convention. You can also use a **sliding-window**, **token-bucket**, or **concurrency** limiter depending on your traffic patterns — see the [official docs](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit).

### Authentication & Authorization

NeedleCrud controllers are plain ASP.NET Core controllers, so every built-in authentication/authorization mechanism works without any NeedleCrud-specific configuration.

**Step 1 — Register JWT Bearer authentication** (or any other scheme):

```csharp
// Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// ...

app.UseAuthentication();
app.UseAuthorization();
```

**Step 2 — Protect individual controllers:**

```csharp
// Require any authenticated user
[Authorize]
[Route("api/books")]
public class BooksController(ICrudDbService<LibraryDbContext, Book, Guid> service)
    : CrudController<Book, Guid>(service) { }

// Require a specific role
[Authorize(Roles = "Admin,Librarian")]
[Route("api/loans")]
public class LoansController(ICrudDbService<LibraryDbContext, Loan, Guid> service)
    : CrudController<Loan, Guid>(service) { }

// Named policy (policy defined in AddAuthorization())
[Authorize(Policy = "LibraryStaff")]
[Route("api/users")]
public class UsersController(ICrudDbService<LibraryDbContext, User, Guid> service)
    : CrudController<User, Guid>(service) { }
```

**Step 3 — Mix anonymous read access with protected writes:**

```csharp
[Authorize]                     // default: auth required
[Route("api/books")]
public class BooksController(ICrudDbService<LibraryDbContext, Book, Guid> service)
    : CrudController<Book, Guid>(service)
{
    // Override read operations to allow anonymous access
    [AllowAnonymous]
    public override Task<Book[]> GetAll() => base.GetAll();

    [AllowAnonymous]
    public override Task<Book> GetById([FromRoute] Guid id) => base.GetById(id);

    [AllowAnonymous]
    public override Task<PagedList<Book>> GetPaged([FromQuery] PagedListQuery query)
        => base.GetPaged(query);

    // Create / Update / Delete inherit [Authorize] from the class
}
```

### Public APIs — What to Protect

| Concern | Recommended approach |
|---------|----------------------|
| Unauthenticated access | `[Authorize]` on controller / action |
| Role-based access | `[Authorize(Roles = "...")]` |
| Policy-based access | `[Authorize(Policy = "...")]` + `AddAuthorization()` |
| Brute-force / DDoS | `AddRateLimiter()` + `app.UseRateLimiter()` |
| Large data exports | Enforce `pageSize` limit in service/repository |
| Injection via filter values | NeedleCrud validates property names via reflection |
| HTTPS | `app.UseHttpsRedirection()` (already in template) |
| CORS | `AddCors()` + `app.UseCors()` — restrict allowed origins |

> **See the sample project** (`LabEG.NeedleCrud.Sample/Program.cs`) for ready-to-uncomment code snippets for both rate limiting and JWT authentication.

---

## ⚙️ Advanced Configuration

### Configuring NeedleCrudSettings

NeedleCrud works out of the box with sensible defaults — no configuration required. When you need to customize the built-in limits, register `NeedleCrudSettings` through the standard ASP.NET Core options system.

**Option 1 — via `appsettings.json`:**

```json
{
  "NeedleCrud": {
    "MaxGetAllCount": 500,
    "MaxPageSize":    50,
    "MaxFilterCount": 20,
    "MaxSortCount":   10,
    "MaxGraphDepth":  8
  }
}
```

```csharp
// Program.cs
builder.Services.Configure<NeedleCrudSettings>(
    builder.Configuration.GetSection("NeedleCrud"));
```

**Option 2 — directly in code:**

```csharp
// Program.cs
builder.Services.Configure<NeedleCrudSettings>(options =>
{
    options.MaxGetAllCount = 500;   // default: 1000
    options.MaxPageSize    = 50;    // default: 100
    options.MaxFilterCount = 20;    // default: 50
    options.MaxSortCount   = 10;    // default: 50
    options.MaxGraphDepth  = 8;     // default: 16
});
```

When `NeedleCrudSettings` is not registered, both `CrudController` and `CrudDbRepository` fall back to the default values automatically — no extra constructor parameters are needed in your controllers:

```csharp
// Works with or without NeedleCrudSettings registered in DI
[Route("api/books")]
public class BooksController(ICrudDbService<MyDbContext, Book, Guid> service)
    : CrudController<Book, Guid>(service) { }
```

| Setting | Default | Description |
|---------|---------|-------------|
| `MaxGetAllCount` | 1000 | Maximum entities returned by `GET /` |
| `MaxPageSize` | 100 | Maximum allowed `pageSize` query parameter |
| `MaxFilterCount` | 50 | Maximum number of filter conditions |
| `MaxSortCount` | 50 | Maximum number of sort conditions |
| `MaxGraphDepth` | 16 | Maximum depth of nested eager-loading graph |

---

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
