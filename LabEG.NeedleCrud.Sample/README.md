# LabEG.NeedleCrud.Sample

Interactive sample application demonstrating **NeedleCrud** library capabilities with a complete Library Management API.

## 🎮 Quick Start with Docker

The fastest way to try NeedleCrud:

```bash
docker pull labeg/needlecrud-sample:latest
docker run -p 8080:8080 labeg/needlecrud-sample:latest
```

Open **http://localhost:8080** for interactive Swagger UI.

## 🏃 Run Locally

### Prerequisites
- .NET 8.0 SDK or higher

### Steps

```bash
# Clone repository
git clone https://github.com/LabEG/needle-crud.git
cd needle-crud/LabEG.NeedleCrud.Sample

# Run application
dotnet run

# Or with hot reload
dotnet watch run
```

Navigate to **https://localhost:5001** or **http://localhost:5000**

## 📚 Sample Data

The application uses an **InMemory database** with pre-seeded library data:

- 📖 **100 Books** - Various genres, authors, page counts
- ✍️ **20 Authors** - From different countries
- 👤 **10 Users** - Library members
- 📋 **10 Categories** - Fiction, Science, History, etc.
- 📝 **50+ Reviews** - Book ratings and comments
- 🔖 **Active Loans** - Currently borrowed books

Data is **automatically regenerated** on each restart.

## 🎯 Available Endpoints

### Books (`/api/books`)

**Basic Operations:**
```http
GET    /api/books              # Get all books
GET    /api/books/{id}         # Get book by ID
POST   /api/books              # Create new book
PUT    /api/books/{id}         # Update book
DELETE /api/books/{id}         # Delete book
```

**Advanced Queries:**
```http
GET /api/books/paged           # Paginated list with filtering/sorting
GET /api/books/{id}/graph      # Get book with related entities
```

**Example Queries:**

```http
# Books with more than 300 pages, sorted by title
GET /api/books/paged?filter=pageCount~>=~300&sort=title~asc

# Available books in English
GET /api/books/paged?filter=isAvailable~=~true,language~=~English

# Search by title (case-insensitive)
GET /api/books/paged?filter=title~ilike~Harry

# Get book with author and reviews
GET /api/books/paged?graph={"author":null,"reviews":{"user":null}}

# Complex query: Available books, 200+ pages, sorted by rating
GET /api/books/paged?
  pageSize=20&
  filter=isAvailable~=~true,pageCount~>=~200&
  sort=rating~desc,title~asc&
  graph={"author":null,"category":null}
```

### Authors (`/api/authors`)

```http
# Get all authors
GET /api/authors

# Filter by country
GET /api/authors/paged?filter=country~=~USA

# Get author with all their books
GET /api/authors/paged?graph={"books":null}

# Search by name
GET /api/authors/paged?filter=name~ilike~Smith
```

### Categories (`/api/categories`)

```http
# Get all categories
GET /api/categories

# Get category with books
GET /api/categories/paged?graph={"books":null}

# Filter by name
GET /api/categories/paged?filter=name~=~Fiction
```

### Users (`/api/users`)

```http
# Get all users
GET /api/users

# Get user with active loans
GET /api/users/paged?graph={"loans":{"book":null}}

# Search by email
GET /api/users/paged?filter=email~ilike~gmail
```

### Reviews (`/api/reviews`)

```http
# Get all reviews
GET /api/reviews

# High-rated reviews (4+ stars)
GET /api/reviews/paged?filter=rating~>=~4&sort=rating~desc

# Get review with book and user info
GET /api/reviews/paged?graph={"book":null,"user":null}
```

### Loans (`/api/loans`)

```http
# Get all loans
GET /api/loans

# Active loans (not yet returned)
GET /api/loans/paged?filter=returnDate~=~null

# Get loan with book and user details
GET /api/loans/paged?graph={"book":{"author":null},"user":null}
```

## 🔍 Query Examples by Use Case

### Searching & Filtering

```http
# Find all science fiction books (level-2 navigation property)
GET /api/books/paged?filter=category.name~=~Science Fiction

# Books written by an English author (level-2 navigation property)
GET /api/books/paged?filter=author.country~=~UK

# Books published after 2020 with high ratings
GET /api/books/paged?filter=publishYear~>~2020,rating~>=~4.5

# Available books with less than 200 pages (quick reads)
GET /api/books/paged?filter=isAvailable~=~true,pageCount~<~200

# Reviews for books from a UK author (level-3 navigation property)
GET /api/reviews/paged?filter=book.author.country~=~UK

# High-rated reviews for fiction books (mixed: direct + level-2 property)
GET /api/reviews/paged?filter=rating~>=~4,book.category.name~=~Fiction

# Active loans for books by a specific author (level-3 navigation property)
GET /api/loans/paged?filter=book.author.name~ilike~Tolkien&graph={"book":{"author":null},"user":null}
```

### Sorting

```http
# Newest books first
GET /api/books/paged?sort=publishYear~desc,title~asc

# Most popular books (by rating, then by reviews count)
GET /api/books/paged?sort=rating~desc,reviewsCount~desc

# Alphabetically by title
GET /api/books/paged?sort=title~asc

# Sort by navigation property (level 2) — books ordered by author name
GET /api/books/paged?sort=author.name~asc,title~asc

# Sort by navigation property (level 2) — books ordered by category, then title
GET /api/books/paged?sort=category.name~asc,title~asc

# Sort by navigation property (level 3) — reviews ordered by book's author country
GET /api/reviews/paged?sort=book.author.country~asc,book.title~asc

# Sort by navigation property (level 3) combined with filter
GET /api/loans/paged?filter=book.category.name~=~Fiction&sort=book.author.name~asc,book.title~asc&graph={"book":{"author":null},"user":null}
```

### Pagination

```http
# First page (20 items)
GET /api/books/paged?pageSize=20&pageNumber=1

# Second page
GET /api/books/paged?pageSize=20&pageNumber=2

# Large page for exports
GET /api/books/paged?pageSize=100&pageNumber=1
```

### Eager Loading (Graph)

```http
# Book with author information
GET /api/books/paged?graph={"author":null}

# Book with author and their country
GET /api/books/paged?graph={"author":{"country":null}}

# Book with all related entities
GET /api/books/paged?graph={"author":null,"category":null,"reviews":{"user":null},"loans":null}

# Single book with relations
GET /api/books/{id}/graph?graph={"author":null,"reviews":null}
```

### Complex Real-World Queries

```http
# Find available fiction books, 200-400 pages, sorted by rating
GET /api/books/paged?
  pageSize=20&
  filter=isAvailable~=~true,category.name~=~Fiction,pageCount~>=~200,pageCount~<=~400&
  sort=rating~desc,title~asc

# Get user's borrowed books with author info
GET /api/loans/paged?
  filter=userId~=~{userId},returnDate~=~null&
  graph={"book":{"author":null},"user":null}

# Top-rated books by a specific author
GET /api/books/paged?
  filter=author.name~like~Rowling,rating~>=~4&
  sort=rating~desc&
  graph={"author":null,"reviews":null}
```

## 📖 Understanding the Response

### Paginated Response Structure

```json
{
  "items": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "title": "Sample Book",
      "pageCount": 350,
      "isAvailable": true,
      "author": {
        "id": "...",
        "name": "John Doe"
      }
    }
  ],
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

## 🐳 Docker

### Build Image

```bash
docker build -t needlecrud-sample -f LabEG.NeedleCrud.Sample/Dockerfile .
```

### Run Container

```bash
docker run -p 8080:8080 needlecrud-sample
```

### Docker Compose

```yaml
version: '3.8'
services:
  needlecrud-sample:
    image: labeg/needlecrud-sample:latest
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
```

## ⚙️ Configuring NeedleCrudSettings

NeedleCrud works out of the box with default limits. To customize them, register `NeedleCrudSettings` in `Program.cs`:

**Via `appsettings.json`:**

```json
// appsettings.json
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

**Or directly in code:**

```csharp
// Program.cs
builder.Services.Configure<NeedleCrudSettings>(options =>
{
    options.MaxGetAllCount = 500;
    options.MaxPageSize    = 50;
    options.MaxFilterCount = 20;
    options.MaxSortCount   = 10;
    options.MaxGraphDepth  = 8;
});
```

If `NeedleCrudSettings` is not registered, default values are used automatically — controllers require no extra constructor parameters:

```csharp
[Route("api/books")]
public class BooksController(ICrudDbService<LibraryDbContext, Book, Guid> service)
    : CrudController<Book, Guid>(service) { }
```

| Setting | Default | Description |
|---------|---------|-------------|
| `MaxGetAllCount` | 1000 | Maximum entities returned by `GET /` |
| `MaxPageSize` | 100 | Maximum `pageSize` query parameter value |
| `MaxFilterCount` | 50 | Maximum number of filter conditions per request |
| `MaxSortCount` | 50 | Maximum number of sort conditions per request |
| `MaxGraphDepth` | 16 | Maximum depth of nested eager-loading graph |

---

## 🛠️ Implementation Details

### Database Context

Uses **InMemory database** for easy testing without external dependencies:

```csharp
builder.Services.AddDbContextPool<LibraryDbContext>(options =>
{
    options.UseInMemoryDatabase("NeedleCrudSample");
});
```

### Service Registration

```csharp
builder.Services.AddScoped(typeof(ICrudDbRepository<,,>), typeof(CrudDbRepository<,,>));
builder.Services.AddScoped(typeof(ICrudDbService<,,>), typeof(CrudDbService<,,>));
```

### Controller Example

```csharp
[Route("api/books")]
public class BooksController(ICrudDbService<LibraryDbContext, Book, Guid> service)
    : CrudController<Book, Guid>(service)
{
}
```

That's it! **3 lines of code** for a complete CRUD API.

### Data Seeding

Test data is automatically generated on startup using `TestDataGenerator`:
- Realistic book titles and author names
- Random ratings and reviews
- Proper entity relationships
- Consistent data across runs

## 💡 Tips

1. **Use Swagger UI** - Interactive API documentation with live testing
2. **Try different filters** - Experiment with operators and combinations
3. **Test pagination** - See how metadata changes with different page sizes
4. **Explore graphs** - Load related entities with nested includes
5. **Check performance** - InMemory is fast; try with real database for production

## 🔗 Related Projects

- [Main Documentation](../README.md)
- [Library Documentation](../LabEG.NeedleCrud/README.md)
- [Benchmarks](../LabEG.NeedleCrud.Benchmarks/README.md)

## 📝 License

MIT License - see [LICENSE](../LICENSE) for details.

---

**Made with ❤️ by LabEG**
