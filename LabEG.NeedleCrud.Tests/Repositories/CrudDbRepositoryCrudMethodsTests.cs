using LabEG.NeedleCrud.Models.Exceptions;
using LabEG.NeedleCrud.Models.ViewModels;
using LabEG.NeedleCrud.Repositories;
using LabEG.NeedleCrud.Settings;
using LabEG.NeedleCrud.TestsFixtures.BLL.Entities;
using LabEG.NeedleCrud.TestsFixtures.DAL;
using LabEG.NeedleCrud.TestsFixtures.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LabEG.NeedleCrud.Tests.Repositories;

/// <summary>
/// Tests for CrudDbRepository basic CRUD operations
/// </summary>
public class CrudDbRepositoryTests : IDisposable
{
    private readonly LibraryDbContext _context;
    private readonly CrudDbRepository<LibraryDbContext, Book, Guid> _repository;
    private readonly TestDataSet _testData;

    public CrudDbRepositoryTests()
    {
        // Create in-memory database for tests
        _context = LibraryDbContextFactory.Create(DatabaseProvider.InMemory, $"TestDb_{Guid.NewGuid()}");
        _repository = new CrudDbRepository<LibraryDbContext, Book, Guid>(_context);

        // Generate test data
        _testData = TestDataGenerator.Generate(seed: 42);

        // Seed database
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

    #region Create Tests

    [Fact]
    public async Task Create_ShouldAddBookToDatabase()
    {
        // Arrange
        Book newBook = _testData.Books[0];

        // Act
        Book createdBook = await _repository.Create(newBook);
        await _context.SaveChangesAsync();

        // Assert
        Assert.NotEqual(Guid.Empty, createdBook.Id);

        Book? dbBook = await _context.Books.FindAsync(createdBook.Id);
        Assert.NotNull(dbBook);
        Assert.Equal(newBook.Title, dbBook.Title);
        Assert.Equal(newBook.ISBN, dbBook.ISBN);
        Assert.Equal(newBook.AuthorId, dbBook.AuthorId);
        Assert.Equal(newBook.CategoryId, dbBook.CategoryId);
    }

    [Fact]
    public async Task Create_ShouldResetIdToDefault()
    {
        // Arrange
        Book newBook = _testData.Books[0];
        Guid originalId = Guid.NewGuid();
        newBook.Id = originalId;

        // Act
        Book createdBook = await _repository.Create(newBook);
        await _context.SaveChangesAsync();

        // Assert
        // After SaveChanges, EF Core generates a new Guid ID (not empty)
        Assert.NotEqual(originalId, createdBook.Id);
        Assert.NotEqual(Guid.Empty, createdBook.Id);
    }

    [Fact]
    public async Task Create_MultipleBooksWithSameData_ShouldSucceed()
    {
        // Arrange
        Book book1 = _testData.Books[0];
        Book book2 = new()
        {
            Title = book1.Title,
            ISBN = "9999999999999", // Different ISBN to avoid unique constraint
            AuthorId = book1.AuthorId,
            CategoryId = book1.CategoryId,
            PublicationDate = book1.PublicationDate,
            PageCount = book1.PageCount,
            Publisher = book1.Publisher,
            Language = book1.Language,
            IsAvailable = book1.IsAvailable
        };

        // Act
        await _repository.Create(book1);
        await _repository.Create(book2);
        await _context.SaveChangesAsync();

        // Assert
        GetAllResult<Book> allResult = await _repository.GetAll();
        Assert.Equal(2, allResult.Items.Length);
        Assert.Equal(2, allResult.TotalCount);
    }

    #endregion

    #region GetById Tests

    [Fact]
    public async Task GetById_ExistingBook_ShouldReturnBook()
    {
        // Arrange
        Book book = _testData.Books[0];
        await _repository.Create(book);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        Guid bookId = await _context.Books.Select(b => b.Id).FirstAsync();

        // Act
        Book result = await _repository.GetById(bookId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bookId, result.Id);
        Assert.Equal(book.Title, result.Title);
        Assert.Equal(book.ISBN, result.ISBN);
    }

    [Fact]
    public async Task GetById_NonExistingBook_ShouldThrowException()
    {
        // Arrange
        Guid nonExistingId = Guid.NewGuid();

        // Act & Assert
        ObjectNotFoundNeedleCrudException exception = await Assert.ThrowsAsync<ObjectNotFoundNeedleCrudException>(
            () => _repository.GetById(nonExistingId)
        );

        Assert.Contains(nonExistingId.ToString(), exception.Message);
        Assert.Contains("Book", exception.Message);
    }

    [Fact]
    public async Task GetById_AfterDelete_ShouldThrowException()
    {
        // Arrange
        Book book = _testData.Books[0];
        await _repository.Create(book);
        await _context.SaveChangesAsync();

        Guid bookId = await _context.Books.Select(b => b.Id).FirstAsync();
        await _repository.Delete(bookId);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act & Assert
        await Assert.ThrowsAsync<ObjectNotFoundNeedleCrudException>(
            () => _repository.GetById(bookId)
        );
    }

    #endregion

    #region GetAll Tests

    [Fact]
    public async Task GetAll_EmptyDatabase_ShouldReturnEmptyArray()
    {
        // Act
        GetAllResult<Book> result = await _repository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
        Assert.False(result.IsTruncated);
    }

    [Fact]
    public async Task GetAll_WithBooks_ShouldReturnAllBooks()
    {
        // Arrange
        int booksToAdd = 5;
        for (int i = 0; i < booksToAdd; i++)
        {
            await _repository.Create(_testData.Books[i]);
        }
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        GetAllResult<Book> result = await _repository.GetAll();

        // Assert
        Assert.Equal(booksToAdd, result.Items.Length);
        Assert.Equal(booksToAdd, result.TotalCount);
        Assert.False(result.IsTruncated);
    }

    [Fact]
    public async Task GetAll_ShouldReturnArray_NotList()
    {
        // Arrange
        await _repository.Create(_testData.Books[0]);
        await _context.SaveChangesAsync();

        // Act
        GetAllResult<Book> result = await _repository.GetAll();

        // Assert
        Assert.IsType<Book[]>(result.Items);
    }

    [Fact]
    public async Task GetAll_AfterAddingAndDeletingBooks_ShouldReturnCorrectCount()
    {
        // Arrange
        for (int i = 0; i < 3; i++)
        {
            await _repository.Create(_testData.Books[i]);
        }
        await _context.SaveChangesAsync();

        Guid firstBookId = await _context.Books.Select(b => b.Id).FirstAsync();
        await _repository.Delete(firstBookId);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        GetAllResult<Book> result = await _repository.GetAll();

        // Assert
        Assert.Equal(2, result.Items.Length);
        Assert.Equal(2, result.TotalCount);
        Assert.DoesNotContain(result.Items, b => b.Id == firstBookId);
    }

    [Fact]
    public async Task GetAll_WhenTruncated_ShouldReportCorrectTotalCount()
    {
        // Arrange: configure a very small MaxGetAllCount
        NeedleCrudSettings settings = new() { MaxGetAllCount = 2 };
        CrudDbRepository<LibraryDbContext, Book, Guid> smallMaxRepo =
            new(_context, Options.Create(settings));

        for (int i = 0; i < 5; i++)
        {
            await _repository.Create(_testData.Books[i]);
        }
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        GetAllResult<Book> result = await smallMaxRepo.GetAll();

        // Assert
        Assert.Equal(2, result.Items.Length);  // capped by MaxGetAllCount
        Assert.Equal(5, result.TotalCount);    // true total
        Assert.True(result.IsTruncated);
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Update_ExistingBook_ShouldModifyBook()
    {
        // Arrange
        Book book = _testData.Books[0];
        await _repository.Create(book);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        Guid bookId = await _context.Books.Select(b => b.Id).FirstAsync();

        Book updatedBook = new()
        {
            Id = bookId,
            Title = "Updated Title",
            ISBN = "9999999999999",
            AuthorId = book.AuthorId,
            CategoryId = book.CategoryId,
            PublicationDate = DateTime.UtcNow,
            PageCount = 500,
            Publisher = "Updated Publisher",
            Language = "Updated Language",
            IsAvailable = false
        };

        // Act
        await _repository.Update(bookId, updatedBook);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Assert
        Book result = await _repository.GetById(bookId);
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("9999999999999", result.ISBN);
        Assert.Equal(500, result.PageCount);
        Assert.Equal("Updated Publisher", result.Publisher);
        Assert.Equal("Updated Language", result.Language);
        Assert.False(result.IsAvailable);
    }

    [Fact]
    public async Task Update_NonExistingBook_ShouldThrowException()
    {
        // Arrange
        Guid nonExistingId = Guid.NewGuid();
        Book book = _testData.Books[0];
        book.Id = nonExistingId;

        // Act & Assert
        ObjectNotFoundNeedleCrudException exception = await Assert.ThrowsAsync<ObjectNotFoundNeedleCrudException>(
            () => _repository.Update(nonExistingId, book)
        );

        Assert.Contains(nonExistingId.ToString(), exception.Message);
        Assert.Contains("Book", exception.Message);
    }

    [Fact]
    public async Task Update_ShouldSetIdToProvidedValue()
    {
        // Arrange
        Book book = _testData.Books[0];
        await _repository.Create(book);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        Guid bookId = await _context.Books.Select(b => b.Id).FirstAsync();

        Book updatedBook = new()
        {
            Id = Guid.NewGuid(), // Different ID
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

        // Act
        await _repository.Update(bookId, updatedBook);

        // Assert
        Assert.Equal(bookId, updatedBook.Id);
    }

    [Fact]
    public async Task Update_PartialData_ShouldUpdateOnlyProvidedFields()
    {
        // Arrange
        Book originalBook = _testData.Books[0];
        await _repository.Create(originalBook);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        Guid bookId = await _context.Books.Select(b => b.Id).FirstAsync();

        Book updatedBook = new()
        {
            Id = bookId,
            Title = "New Title Only",
            ISBN = originalBook.ISBN,
            AuthorId = originalBook.AuthorId,
            CategoryId = originalBook.CategoryId,
            PublicationDate = originalBook.PublicationDate,
            PageCount = originalBook.PageCount,
            Publisher = originalBook.Publisher,
            Language = originalBook.Language,
            IsAvailable = originalBook.IsAvailable
        };

        // Act
        await _repository.Update(bookId, updatedBook);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Assert
        Book result = await _repository.GetById(bookId);
        Assert.Equal("New Title Only", result.Title);
        Assert.Equal(originalBook.ISBN, result.ISBN);
        Assert.Equal(originalBook.PageCount, result.PageCount);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_ExistingBook_ShouldRemoveFromDatabase()
    {
        // Arrange
        Book book = _testData.Books[0];
        await _repository.Create(book);
        await _context.SaveChangesAsync();

        Guid bookId = await _context.Books.Select(b => b.Id).FirstAsync();

        // Act
        await _repository.Delete(bookId);
        await _context.SaveChangesAsync();

        // Assert
        Book? dbBook = await _context.Books.FindAsync(bookId);
        Assert.Null(dbBook);
    }

    [Fact]
    public async Task Delete_NonExistingBook_ShouldThrowException()
    {
        // Arrange
        Guid nonExistingId = Guid.NewGuid();

        // Act & Assert
        ObjectNotFoundNeedleCrudException exception = await Assert.ThrowsAsync<ObjectNotFoundNeedleCrudException>(
            () => _repository.Delete(nonExistingId)
        );

        Assert.Contains(nonExistingId.ToString(), exception.Message);
        Assert.Contains("Book", exception.Message);
    }

    [Fact]
    public async Task Delete_AfterDeletion_GetAllShouldNotContainDeletedBook()
    {
        // Arrange
        for (int i = 0; i < 3; i++)
        {
            await _repository.Create(_testData.Books[i]);
        }
        await _context.SaveChangesAsync();

        Guid[] allIds = await _context.Books.Select(b => b.Id).ToArrayAsync();
        Guid idToDelete = allIds[1];

        // Act
        await _repository.Delete(idToDelete);
        await _context.SaveChangesAsync();

        GetAllResult<Book> allResult = await _repository.GetAll();
        Book[] remainingBooks = allResult.Items;

        // Assert
        Assert.Equal(2, remainingBooks.Length);
        Assert.DoesNotContain(remainingBooks, b => b.Id == idToDelete);
        Assert.Contains(remainingBooks, b => b.Id == allIds[0]);
        Assert.Contains(remainingBooks, b => b.Id == allIds[2]);
    }

    [Fact]
    public async Task Delete_MultipleTimes_ShouldThrowOnSecondAttempt()
    {
        // Arrange
        Book book = _testData.Books[0];
        await _repository.Create(book);
        await _context.SaveChangesAsync();

        Guid bookId = await _context.Books.Select(b => b.Id).FirstAsync();

        // Act
        await _repository.Delete(bookId);
        await _context.SaveChangesAsync();

        // Assert
        await Assert.ThrowsAsync<ObjectNotFoundNeedleCrudException>(
            () => _repository.Delete(bookId)
        );
    }

    #endregion
}
