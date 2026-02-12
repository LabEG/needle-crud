# Contributing to NeedleCrud

Thank you for your interest in contributing! This document provides guidelines for contributing to this project.

## Code of Conduct

By participating in this project, you agree to abide by our [Code of Conduct](./CODE_OF_CONDUCT.md).

## How to Contribute

### Reporting Bugs

Before creating bug reports, please check the existing issues. When creating a bug report, include:

- **Clear title and description**
- **Steps to reproduce** the problem
- **Expected behavior** vs **actual behavior**
- **Code samples** that demonstrate the issue
- **Environment details** (.NET version, Entity Framework Core version, OS)

### Suggesting Enhancements

Enhancement suggestions are welcome! Please provide:

- **Clear description** of the enhancement
- **Use cases** and why it would be useful
- **Possible implementation** approach (if you have ideas)

### Pull Requests

1. **Fork the repository** and create your branch from `master`
2. **Make your changes** following our coding standards
3. **Test your changes** by running `dotnet test`
4. **Commit your changes** using [Conventional Commits](https://www.conventionalcommits.org/):
   - `feat:` for new features
   - `fix:` for bug fixes
   - `docs:` for documentation changes
   - `chore:` for maintenance tasks
   - `refactor:` for code refactoring
   - `test:` for test additions or modifications
   - `perf:` for performance improvements
5. **Push to your fork** and submit a pull request

## Development Setup

### Prerequisites

- .NET 8.0, 9.0, or 10.0 SDK
- Docker (optional, for running sample application)
- Your favorite IDE (Visual Studio, Visual Studio Code, or JetBrains Rider)

### Setup Steps

```bash
# Clone your fork
git clone https://github.com/YOUR_USERNAME/needle-crud.git
cd needle-crud

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Run sample project
cd LabEG.NeedleCrud.Sample
dotnet run
```

### Running Benchmarks

```bash
cd LabEG.NeedleCrud.Benchmarks
dotnet run -c Release
```

## Coding Standards

This project uses [@labeg/code-style](https://github.com/LabEG/code-style) for consistent code formatting. Key principles:

### Code Style

- **Line length**: Maximum 120 characters
- **Indentation**: 4 spaces
- **Quotes**: Use double quotes `"` for strings
- **Semicolons**: Always use semicolons
- **Braces**: Always use braces for control structures
- **Naming conventions**: Follow C# naming conventions (PascalCase for classes/methods, camelCase for parameters)

### Example

```csharp
// Good
public class BookService : ICrudDbService<Book, Guid>
{
    private readonly ICrudDbRepository<LibraryDbContext, Book, Guid> _repository;

    public BookService(ICrudDbRepository<LibraryDbContext, Book, Guid> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Book> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty", nameof(id));
        }

        return await _repository.GetByIdAsync(id);
    }
}

// Bad
public class BookService : ICrudDbService<Book, Guid>
{
    private readonly ICrudDbRepository<LibraryDbContext, Book, Guid> repo;

    public BookService(ICrudDbRepository<LibraryDbContext, Book, Guid> repository)
    {
        repo = repository;
    }

    public async Task<Book> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty", nameof(id));
        return await repo.GetByIdAsync(id);
    }
}
```

### Commit Messages

Follow [Conventional Commits](https://www.conventionalcommits.org/):

```bash
feat: add support for custom filter operators
fix: correct pagination calculation for empty result sets
docs: update README with graph query examples
chore: upgrade Entity Framework Core to 10.0
test: add integration tests for CrudDbRepository
perf: optimize Expression Tree compilation for filters
```

## Testing

Before submitting your PR:

```bash
# Run all tests
dotnet test

# Run tests with coverage (if configured)
dotnet test /p:CollectCoverage=true

# Run benchmarks (for performance-related changes)
cd LabEG.NeedleCrud.Benchmarks
dotnet run -c Release
```

All tests must pass before your PR can be merged.

### Writing Tests

- Use xUnit for unit tests
- Follow AAA pattern (Arrange, Act, Assert)
- Use meaningful test names that describe the scenario
- Test both success and failure cases
- Use test fixtures from `LabEG.NeedleCrud.TestsFixtures` for consistency

Example:

```csharp
[Fact]
public async Task GetPaged_WithValidFilter_ReturnsFilteredResults()
{
    // Arrange
    var repository = CreateRepository();
    var query = new PagedListQuery(
        pageSize: 10,
        pageNumber: 1,
        filter: "pageCount~>=~300"
    );

    // Act
    var result = await repository.GetPagedAsync(query);

    // Assert
    Assert.All(result.Items, book => Assert.True(book.PageCount >= 300));
}
```

## Project Structure

- **LabEG.NeedleCrud** - Main library with core functionality
- **LabEG.NeedleCrud.Sample** - Sample application demonstrating usage
- **LabEG.NeedleCrud.Tests** - Unit and integration tests
- **LabEG.NeedleCrud.TestsFixtures** - Shared test utilities and fixtures
- **LabEG.NeedleCrud.Benchmarks** - Performance benchmarks

## Areas for Contribution

We welcome contributions in these areas:

### High Priority
- Performance optimizations
- Additional filter operators
- Support for more EF Core features
- Documentation improvements
- Bug fixes

### Medium Priority
- Additional examples and samples
- Integration with other libraries
- Enhanced error messages
- Code refactoring for better maintainability

### Nice to Have
- Support for additional databases
- Advanced query features
- Developer tools and utilities
- Video tutorials and guides

## Review Process

1. **Automated checks** run on every PR (tests, build verification)
2. **Manual review** by maintainers
3. **Feedback** may be provided - please address comments
4. **Approval** - once approved, your PR will be merged

## Release Process

Releases follow semantic versioning:

1. Maintainer merges PR to `master`
2. Version is bumped according to change type (major.minor.patch)
3. Changelog is updated
4. NuGet package is published
5. GitHub release is created with release notes

## Documentation

When adding new features:

1. Update relevant README files
2. Add XML documentation comments to public APIs
3. Update code examples if applicable
4. Add entries to CHANGELOG.md (if applicable)

## Questions?

- Open an issue with the `question` label
- Check existing issues and discussions
- Contact maintainers at labeg@mail.ru

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to NeedleCrud! 🎉
