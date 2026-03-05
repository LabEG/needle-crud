using LabEG.NeedleCrud.Models.Exceptions;
using LabEG.NeedleCrud.Repositories;
using LabEG.NeedleCrud.Services;
using LabEG.NeedleCrud.Settings;
using LabEG.NeedleCrud.TestsFixtures.BLL.Entities;
using LabEG.NeedleCrud.TestsFixtures.DAL;
using LabEG.NeedleCrud.TestsFixtures.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LabEG.NeedleCrud.Tests.Repositories;

/// <summary>
/// Tests verifying the behaviour of the <see cref="NeedleCrudSettings.UnitOfWork"/> flag
/// in both <see cref="CrudDbRepository{TDbContext,TEntity,TId}"/> and
/// <see cref="CrudDbService{TDbContext,TEntity,TId}"/>.
/// </summary>
public class CrudDbRepositoryUnitOfWorkTests : IDisposable
{
    private readonly LibraryDbContext _context;
    private readonly TestDataSet _testData;

    public CrudDbRepositoryUnitOfWorkTests()
    {
        _context = LibraryDbContextFactory.Create(DatabaseProvider.InMemory, $"UoWTestDb_{Guid.NewGuid()}");
        _testData = TestDataGenerator.Generate(seed: 7);

        _context.Authors.AddRange(_testData.Authors);
        _context.Categories.AddRange(_testData.Categories);
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    // -------------------------------------------------------------------------
    // Helper: build repository with a given UnitOfWork flag
    // -------------------------------------------------------------------------
    private CrudDbRepository<LibraryDbContext, Book, Guid> BuildRepository(bool unitOfWork)
    {
        IOptions<NeedleCrudSettings> options = Options.Create(new NeedleCrudSettings { UnitOfWork = unitOfWork });
        return new CrudDbRepository<LibraryDbContext, Book, Guid>(_context, options);
    }

    private CrudDbService<LibraryDbContext, Book, Guid> BuildService(bool unitOfWork)
    {
        IOptions<NeedleCrudSettings> options = Options.Create(new NeedleCrudSettings { UnitOfWork = unitOfWork });
        CrudDbRepository<LibraryDbContext, Book, Guid> repo = new(_context, options);
        return new CrudDbService<LibraryDbContext, Book, Guid>(_context, repo, options);
    }

    // =========================================================================
    // Repository — UnitOfWork = true (repository auto-commits)
    // =========================================================================

    [Fact]
    public async Task Repository_Create_UnitOfWork_True_AutoSaves()
    {
        // Arrange
        CrudDbRepository<LibraryDbContext, Book, Guid> repo = BuildRepository(unitOfWork: true);
        Book newBook = _testData.Books[0];

        // Act — no explicit SaveChangesAsync needed
        Book created = await repo.Create(newBook);

        // Assert — entity is already in the database
        _context.ChangeTracker.Clear();
        Book? dbBook = await _context.Books.FindAsync(created.Id);
        Assert.NotNull(dbBook);
        Assert.Equal(newBook.Title, dbBook.Title);
    }

    [Fact]
    public async Task Repository_Update_UnitOfWork_True_AutoSaves()
    {
        // Arrange
        CrudDbRepository<LibraryDbContext, Book, Guid> repo = BuildRepository(unitOfWork: true);
        Book book = _testData.Books[0];
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        Guid savedId = await _context.Books.Select(b => b.Id).FirstAsync();
        _context.ChangeTracker.Clear();

        Book updatedBook = new()
        {
            Id = savedId,
            Title = "Updated Title",
            ISBN = book.ISBN,
            AuthorId = book.AuthorId,
            CategoryId = book.CategoryId,
            PublicationDate = book.PublicationDate,
            PageCount = book.PageCount,
            Publisher = book.Publisher,
            Language = book.Language,
            IsAvailable = book.IsAvailable
        };

        // Act — no explicit SaveChangesAsync needed
        await repo.Update(savedId, updatedBook);

        // Assert
        _context.ChangeTracker.Clear();
        Book? dbBook = await _context.Books.FindAsync(savedId);
        Assert.NotNull(dbBook);
        Assert.Equal("Updated Title", dbBook.Title);
    }

    [Fact]
    public async Task Repository_Delete_UnitOfWork_True_AutoSaves()
    {
        // Arrange
        CrudDbRepository<LibraryDbContext, Book, Guid> repo = BuildRepository(unitOfWork: true);
        Book book = _testData.Books[0];
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        Guid savedId = await _context.Books.Select(b => b.Id).FirstAsync();
        _context.ChangeTracker.Clear();

        // Act — no explicit SaveChangesAsync needed
        await repo.Delete(savedId);

        // Assert
        _context.ChangeTracker.Clear();
        Book? dbBook = await _context.Books.FindAsync(savedId);
        Assert.Null(dbBook);
    }

    [Fact]
    public async Task Repository_Update_UnitOfWork_True_NonExistingId_ThrowsObjectNotFound()
    {
        // Arrange
        CrudDbRepository<LibraryDbContext, Book, Guid> repo = BuildRepository(unitOfWork: true);
        Guid nonExistingId = Guid.NewGuid();
        Book fakeBook = new()
        {
            Id = nonExistingId,
            Title = "Ghost",
            ISBN = "0000000000000",
            AuthorId = _testData.Authors[0].Id,
            CategoryId = _testData.Categories[0].Id
        };

        // Act & Assert
        await Assert.ThrowsAsync<ObjectNotFoundNeedleCrudException>(
            () => repo.Update(nonExistingId, fakeBook));
    }

    // =========================================================================
    // Repository — UnitOfWork = false (default, caller manages SaveChanges)
    // =========================================================================

    [Fact]
    public async Task Repository_Create_UnitOfWork_False_DoesNotAutoSave()
    {
        // Arrange
        CrudDbRepository<LibraryDbContext, Book, Guid> repo = BuildRepository(unitOfWork: false);
        Book newBook = _testData.Books[0];

        // Act — create without explicit SaveChangesAsync
        await repo.Create(newBook);

        // Assert — clear the change tracker so EF doesn't return the still-staged entity,
        // then verify the in-memory store has no books (SaveChanges was never called).
        _context.ChangeTracker.Clear();
        int count = await _context.Books.CountAsync();
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task Repository_Create_UnitOfWork_False_PersistsAfterExplicitSave()
    {
        // Arrange
        CrudDbRepository<LibraryDbContext, Book, Guid> repo = BuildRepository(unitOfWork: false);
        Book newBook = _testData.Books[0];

        // Act
        Book created = await repo.Create(newBook);
        await _context.SaveChangesAsync();  // explicit commit

        // Assert
        _context.ChangeTracker.Clear();
        Book? dbBook = await _context.Books.FindAsync(created.Id);
        Assert.NotNull(dbBook);
        Assert.Equal(newBook.Title, dbBook.Title);
    }

    // =========================================================================
    // Service — UnitOfWork = true (repository already committed, service skips)
    // =========================================================================

    [Fact]
    public async Task Service_Create_UnitOfWork_True_AutoSavesViaRepository()
    {
        // Arrange
        CrudDbService<LibraryDbContext, Book, Guid> service = BuildService(unitOfWork: true);
        Book newBook = _testData.Books[0];

        // Act
        Book created = await service.Create(newBook);

        // Assert
        _context.ChangeTracker.Clear();
        Book? dbBook = await _context.Books.FindAsync(created.Id);
        Assert.NotNull(dbBook);
        Assert.Equal(newBook.Title, dbBook.Title);
    }

    [Fact]
    public async Task Service_Delete_UnitOfWork_True_AutoSavesViaRepository()
    {
        // Arrange
        CrudDbService<LibraryDbContext, Book, Guid> service = BuildService(unitOfWork: true);
        Book book = _testData.Books[0];
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        Guid savedId = await _context.Books.Select(b => b.Id).FirstAsync();
        _context.ChangeTracker.Clear();

        // Act
        await service.Delete(savedId);

        // Assert
        _context.ChangeTracker.Clear();
        Book? dbBook = await _context.Books.FindAsync(savedId);
        Assert.Null(dbBook);
    }

    // =========================================================================
    // Service — UnitOfWork = false (default, service calls SaveChanges)
    // =========================================================================

    [Fact]
    public async Task Service_Create_UnitOfWork_False_SavesViaService()
    {
        // Arrange
        CrudDbService<LibraryDbContext, Book, Guid> service = BuildService(unitOfWork: false);
        Book newBook = _testData.Books[0];

        // Act — service must call SaveChangesAsync internally
        Book created = await service.Create(newBook);

        // Assert
        _context.ChangeTracker.Clear();
        Book? dbBook = await _context.Books.FindAsync(created.Id);
        Assert.NotNull(dbBook);
        Assert.Equal(newBook.Title, dbBook.Title);
    }

    [Fact]
    public async Task Service_Update_UnitOfWork_False_NonExistingId_ThrowsObjectNotFound()
    {
        // Arrange
        CrudDbService<LibraryDbContext, Book, Guid> service = BuildService(unitOfWork: false);
        Guid nonExistingId = Guid.NewGuid();
        Book fakeBook = new()
        {
            Id = nonExistingId,
            Title = "Ghost",
            ISBN = "0000000000000",
            AuthorId = _testData.Authors[0].Id,
            CategoryId = _testData.Categories[0].Id
        };

        // Act & Assert
        await Assert.ThrowsAsync<ObjectNotFoundNeedleCrudException>(
            () => service.Update(nonExistingId, fakeBook));
    }
}
