using LabEG.NeedleCrud.Infrastructure;
using LabEG.NeedleCrud.Repositories;
using LabEG.NeedleCrud.Services;
using LabEG.NeedleCrud.Benchmarks.BLL;
using LabEG.NeedleCrud.Benchmarks.Fixtures;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure InMemory database
builder.Services.AddDbContextPool<LibraryDbContext>(options =>
{
    options.UseInMemoryDatabase("NeedleCrudSample");
});

// Register LabEG.NeedleCrud library dependencies
builder.Services.AddScoped(typeof(ICrudDbRepository<,,>), typeof(CrudDbRepository<,,>));
builder.Services.AddScoped(typeof(ICrudDbService<,,>), typeof(CrudDbService<,,>));


// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Configure Swagger/OpenAPI with XML documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "NeedleCrud Sample API",
        Version = "v1",
        Description = "Sample API demonstrating NeedleCrud library usage for CRUD operations with advanced filtering, sorting, and pagination"
    });

    // Include XML comments from current project
    string xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Include XML comments from LabEG.NeedleCrud library
    string needleCrudXmlPath = Path.Combine(AppContext.BaseDirectory, "LabEG.NeedleCrud.xml");
    if (File.Exists(needleCrudXmlPath))
    {
        options.IncludeXmlComments(needleCrudXmlPath);
    }

    // Include XML comments from LabEG.NeedleCrud.TestsFixtures
    string fixturesXmlPath = Path.Combine(AppContext.BaseDirectory, "LabEG.NeedleCrud.TestsFixtures.xml");
    if (File.Exists(fixturesXmlPath))
    {
        options.IncludeXmlComments(fixturesXmlPath);
    }
});

WebApplication app = builder.Build();

// Initialize database with test data if empty
using (IServiceScope scope = app.Services.CreateScope())
{
    LibraryDbContext context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

    // Check if database is empty
    if (!context.Users.Any())
    {
        // Create database schema (for InMemory it's automatic, but good practice)
        context.Database.EnsureCreated();

        // Seed with test data
        TestDataGenerator.SeedDatabase(context);
    }
}

// Configure the HTTP request pipeline.
// Add NeedleCrud exception handler middleware
app.UseNeedleCrudExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "NeedleCrud Sample API v1");
    options.RoutePrefix = string.Empty; // Swagger UI at root
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
