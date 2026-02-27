using LabEG.NeedleCrud.Infrastructure;
using LabEG.NeedleCrud.Repositories;
using LabEG.NeedleCrud.Services;
using LabEG.NeedleCrud.Settings;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using LabEG.NeedleCrud.TestsFixtures.Fixtures;
using LabEG.NeedleCrud.TestsFixtures.DAL;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure InMemory database
builder.Services.AddDbContextPool<LibraryDbContext>(options =>
{
    options.UseInMemoryDatabase("NeedleCrudSample");
});

// Register NeedleCrud settings with configuration binding from appsettings.json
builder.Services.Configure<NeedleCrudSettings>(builder.Configuration.GetSection("NeedleCrud"));

// Register LabEG.NeedleCrud library dependencies
builder.Services.AddScoped(typeof(ICrudDbRepository<,,>), typeof(CrudDbRepository<,,>));
builder.Services.AddScoped(typeof(ICrudDbService<,,>), typeof(CrudDbService<,,>));

// ── Rate Limiting ────────────────────────────────────────────────────────────
// NeedleCrud exposes all CRUD endpoints automatically, so it's important to
// protect them with rate limiting to prevent abuse.
//
// The example below uses a fixed-window limiter: each client IP may send
// no more than 100 requests per minute. Requests that exceed the limit get
// a 429 Too Many Requests response.
//
// You can uncomment the UseRateLimiter() call in the pipeline section below
// to activate rate limiting. You can also replace this policy with a sliding-
// window, token-bucket, or concurrency limiter — see Microsoft docs:
// https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100;
        opt.QueueLimit = 0;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Friendly response body for rate-limited requests
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync(
            "Too many requests. Please try again later.", cancellationToken);
    };
});

// ── Authentication & Authorization ──────────────────────────────────────────
// NeedleCrud controllers are plain ASP.NET Core controllers, so standard
// authentication/authorization applies out of the box.
//
// To enable JWT Bearer authentication:
//   1. Add the NuGet package: Microsoft.AspNetCore.Authentication.JwtBearer
//   2. Uncomment the block below and fill in your issuer / audience / key.
//   3. Decorate individual controllers (or the base CrudController) with
//      [Authorize] — or add a global filter via AddControllers().
//
// You can also use any other authentication scheme (cookies, API keys, etc.)
// because NeedleCrud does not interfere with the ASP.NET Core auth pipeline.
//
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer           = true,
//             ValidateAudience         = true,
//             ValidateLifetime         = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer              = builder.Configuration["Jwt:Issuer"],
//             ValidAudience            = builder.Configuration["Jwt:Audience"],
//             IssuerSigningKey         = new SymmetricSecurityKey(
//                 Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
//         };
//     });
//
// builder.Services.AddAuthorization();

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

// Uncomment the line below to enable rate limiting (see configuration above).
// app.UseRateLimiter();

// Uncomment the lines below to enable authentication and authorization
// (see configuration above and [Authorize] attributes on controllers).
// app.UseAuthentication();
// app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "NeedleCrud Sample API v1");
    options.RoutePrefix = string.Empty; // Swagger UI at root
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
