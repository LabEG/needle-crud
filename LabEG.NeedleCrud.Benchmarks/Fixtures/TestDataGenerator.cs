using Bogus;
using LabEG.NeedleCrud.Benchmarks.BLL;
using LabEG.NeedleCrud.Benchmarks.BLL.Entities;

namespace LabEG.NeedleCrud.Benchmarks.Fixtures;

/// <summary>
/// Generator for creating test data for benchmarks
/// </summary>
public static class TestDataGenerator
{
    private const int UsersCount = 10;
    private const int AuthorsCount = 20;
    private const int CategoriesCount = 8;
    private const int BooksCount = 100;
    private const int LoansCount = 80;
    private const int ReviewsCount = 150;

    /// <summary>
    /// Generates complete test dataset
    /// </summary>
    /// <param name="seed">Random seed for reproducible data generation</param>
    /// <returns>Test data set containing all entities</returns>
    public static TestDataSet Generate(int seed = 12345)
    {
        Randomizer.Seed = new Random(seed);

        var users = GenerateUsers();
        var authors = GenerateAuthors();
        var categories = GenerateCategories();
        var books = GenerateBooks(authors, categories);
        var loans = GenerateLoans(users, books);
        var reviews = GenerateReviews(users, books);

        return new TestDataSet
        {
            Users = users,
            Authors = authors,
            Categories = categories,
            Books = books,
            Loans = loans,
            Reviews = reviews
        };
    }

    /// <summary>
    /// Seeds database context with generated test data
    /// </summary>
    /// <param name="context">Database context to seed</param>
    /// <param name="seed">Random seed for reproducible data generation</param>
    public static void SeedDatabase(LibraryDbContext context, int seed = 12345)
    {
        var data = Generate(seed);

        context.Users.AddRange(data.Users);
        context.Authors.AddRange(data.Authors);
        context.Categories.AddRange(data.Categories);
        context.Books.AddRange(data.Books);
        context.Loans.AddRange(data.Loans);
        context.Reviews.AddRange(data.Reviews);

        context.SaveChanges();
    }

    private static List<User> GenerateUsers()
    {
        var faker = new Faker<User>()
            .RuleFor(u => u.Id, f => f.Random.Guid())
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("###-###-####"))
            .RuleFor(u => u.RegistrationDate, f => DateTime.SpecifyKind(f.Date.Past(2), DateTimeKind.Utc))
            .RuleFor(u => u.IsActive, f => f.Random.Bool(0.9f))
            .RuleFor(u => u.Address, f => f.Address.FullAddress());

        return faker.Generate(UsersCount);
    }

    private static List<Author> GenerateAuthors()
    {
        var faker = new Faker<Author>()
            .RuleFor(a => a.Id, f => f.Random.Guid())
            .RuleFor(a => a.FirstName, f => f.Name.FirstName())
            .RuleFor(a => a.LastName, f => f.Name.LastName())
            .RuleFor(a => a.BirthDate, f => DateTime.SpecifyKind(f.Date.Past(80, DateTime.UtcNow.AddYears(-20)), DateTimeKind.Utc))
            .RuleFor(a => a.Country, f => f.Address.Country())
            .RuleFor(a => a.Biography, f => f.Lorem.Paragraphs(2));

        return faker.Generate(AuthorsCount);
    }

    private static List<Category> GenerateCategories()
    {
        var categoryNames = new[]
        {
            "Fiction", "Non-Fiction", "Science Fiction", "Mystery",
            "Romance", "Biography", "History", "Technology"
        };

        var faker = new Faker<Category>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => categoryNames[f.IndexFaker % categoryNames.Length])
            .RuleFor(c => c.Description, f => f.Lorem.Sentence(10))
            .RuleFor(c => c.DisplayOrder, f => f.IndexFaker + 1)
            .RuleFor(c => c.IsActive, f => f.Random.Bool(0.95f));

        return faker.Generate(CategoriesCount);
    }

    private static List<Book> GenerateBooks(List<Author> authors, List<Category> categories)
    {
        var faker = new Faker<Book>()
            .RuleFor(b => b.Id, f => f.Random.Guid())
            .RuleFor(b => b.Title, f => f.Lorem.Sentence(3, 5))
            .RuleFor(b => b.ISBN, f => f.Commerce.Ean13().Substring(0, Math.Min(13, 20)))
            .RuleFor(b => b.AuthorId, f => f.PickRandom(authors).Id)
            .RuleFor(b => b.CategoryId, f => f.PickRandom(categories).Id)
            .RuleFor(b => b.PublicationDate, f => DateTime.SpecifyKind(f.Date.Past(30), DateTimeKind.Utc))
            .RuleFor(b => b.PageCount, f => f.Random.Int(100, 1000))
            .RuleFor(b => b.Publisher, f => f.Company.CompanyName())
            .RuleFor(b => b.Language, f => f.PickRandom("English", "Russian", "French", "German", "Spanish"))
            .RuleFor(b => b.IsAvailable, f => f.Random.Bool(0.7f));

        return faker.Generate(BooksCount);
    }

    private static List<Loan> GenerateLoans(List<User> users, List<Book> books)
    {
        var faker = new Faker<Loan>()
            .RuleFor(l => l.Id, f => f.Random.Guid())
            .RuleFor(l => l.UserId, f => f.PickRandom(users).Id)
            .RuleFor(l => l.BookId, f => f.PickRandom(books).Id)
            .RuleFor(l => l.LoanDate, f => DateTime.SpecifyKind(f.Date.Past(1), DateTimeKind.Utc))
            .RuleFor(l => l.DueDate, (f, l) => l.LoanDate.AddDays(14))
            .RuleFor(l => l.IsReturned, f => f.Random.Bool(0.6f))
            .RuleFor(l => l.ReturnDate, (f, l) => l.IsReturned ? l.LoanDate.AddDays(f.Random.Int(1, 20)) : (DateTime?)null)
            .RuleFor(l => l.LateFee, (f, l) =>
            {
                if (!l.IsReturned || l.ReturnDate == null)
                    return null;

                var daysLate = (l.ReturnDate.Value - l.DueDate).Days;
                return daysLate > 0 ? (decimal?)(daysLate * 0.50m) : null;
            })
            .RuleFor(l => l.Notes, f => f.Random.Bool(0.3f) ? f.Lorem.Sentence() : string.Empty);

        return faker.Generate(LoansCount);
    }

    private static List<Review> GenerateReviews(List<User> users, List<Book> books)
    {
        var faker = new Faker<Review>()
            .RuleFor(r => r.Id, f => f.Random.Guid())
            .RuleFor(r => r.UserId, f => f.PickRandom(users).Id)
            .RuleFor(r => r.BookId, f => f.PickRandom(books).Id)
            .RuleFor(r => r.Rating, f => f.Random.Int(1, 5))
            .RuleFor(r => r.Comment, f => f.Rant.Review())
            .RuleFor(r => r.ReviewDate, f => DateTime.SpecifyKind(f.Date.Past(1), DateTimeKind.Utc))
            .RuleFor(r => r.IsVerified, f => f.Random.Bool(0.8f));

        return faker.Generate(ReviewsCount);
    }
}

/// <summary>
/// Container for test data set
/// </summary>
public class TestDataSet
{
    public List<User> Users { get; set; } = new();
    public List<Author> Authors { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<Book> Books { get; set; } = new();
    public List<Loan> Loans { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}
