# Security Policy

## Supported Versions

We actively support the following versions of NeedleCrud with security updates:

| Version | Supported          |
| ------- | ------------------ |
| 1.x.x   | :white_check_mark: |
| < 1.0   | :x:                |

## Reporting a Vulnerability

We take the security of NeedleCrud seriously. If you discover a security vulnerability, please follow these steps:

### How to Report

1. **DO NOT** open a public GitHub issue for security vulnerabilities
2. Send a detailed report to the repository maintainer via:
   - GitHub Security Advisory: [Report a vulnerability](https://github.com/LabEG/needle-crud/security/advisories/new)
   - Email: labeg@mail.ru with subject "NeedleCrud Security Vulnerability"

### What to Include

Please provide the following information in your report:

- **Description**: A clear description of the vulnerability
- **Impact**: What could an attacker accomplish by exploiting this vulnerability
- **Reproduction**: Step-by-step instructions to reproduce the issue
- **Version**: The version of NeedleCrud affected
- **Environment**: Relevant environment details (.NET version, Entity Framework Core version, database provider, OS)
- **Suggested Fix** (optional): If you have ideas on how to fix the vulnerability

### Response Timeline

- **Initial Response**: Within 48 hours of receiving the report
- **Status Update**: Within 7 days with either a fix timeline or request for more information
- **Resolution**: Security patches will be released as soon as possible, typically within 14 days for critical issues

### Security Update Process

1. The vulnerability is confirmed and assessed
2. A fix is developed and tested
3. A security advisory is prepared
4. A new version is released with the fix
5. The security advisory is published with CVE (if applicable)
6. Affected users are notified through GitHub releases and NuGet

## Security Best Practices

When using NeedleCrud:

### For Package Consumers

- Always use the latest stable version
- Regularly update dependencies using `dotnet outdated` or similar tools
- Review the release notes for security-related updates
- Use `dotnet list package --vulnerable` to check for known vulnerabilities in dependencies
- Validate and sanitize user input before passing to filter/sort parameters
- Implement proper authorization and authentication in your controllers
- Use HTTPS for production deployments

### Input Validation

NeedleCrud processes user-provided query parameters. Always:

```csharp
// Add authorization
[Authorize]
[Route("api/books")]
public class BooksController : CrudController<Book, Guid>
{
    // Override methods to add validation
    public override async Task<PagedList<Book>> GetPaged([FromQuery] PagedListQuery query)
    {
        // Validate page size to prevent resource exhaustion
        if (query.PageSize > 100)
        {
            throw new ValidationException("Page size cannot exceed 100");
        }

        return await base.GetPaged(query);
    }
}
```

### For Contributors

- Follow secure coding practices
- Run vulnerability scans before submitting pull requests
- Never commit sensitive information (connection strings, API keys, passwords, tokens)
- Test changes thoroughly with various configurations
- Review dependencies for known vulnerabilities
- Use parameterized queries (NeedleCrud uses EF Core which protects against SQL injection)

## Dependency Security

This package relies on:
- ASP.NET Core
- Entity Framework Core
- Microsoft.Extensions.* packages

We:

- Monitor security advisories for all dependencies
- Update dependencies promptly when security issues are discovered
- Use automated dependency scanning in our CI/CD pipeline
- Follow semantic versioning to ensure stable updates

## Known Security Considerations

As a CRUD library that processes user input, NeedleCrud:

- **Processes query parameters** - validate and sanitize user input in your application
- **Executes database queries** - uses EF Core parameterized queries to prevent SQL injection
- **Handles entity data** - be cautious with sensitive information in entities
- **Exposes data through APIs** - implement proper authorization and authentication
- **Performs dynamic filtering/sorting** - validates property names against entity model

### Built-in Security Features

NeedleCrud includes:

- **Expression Tree validation** - only valid entity properties can be filtered/sorted
- **Parameterized queries** - all database queries use EF Core parameterization
- **Type-safe operations** - strong typing prevents many injection attacks
- **Exception handling** - custom middleware for consistent error responses

### Security Recommendations

1. **Enable HTTPS** - Always use HTTPS in production
2. **Implement Authentication** - Use ASP.NET Core Identity or similar
3. **Add Authorization** - Apply `[Authorize]` attributes to controllers
4. **Validate Input** - Set reasonable limits for page sizes and filter complexity
5. **Rate Limiting** - Implement rate limiting to prevent abuse
6. **Logging** - Monitor for suspicious query patterns
7. **Database Permissions** - Use principle of least privilege for database access

Example secure configuration:

```csharp
// Add rate limiting
builder.Services.AddRateLimiter(options => { /* configure */ });

// Add authentication
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Configure CORS properly
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins("https://yourdomain.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

## Disclosure Policy

When a security vulnerability is fixed:

1. We will credit the reporter (unless they wish to remain anonymous)
2. Details will be disclosed after a fix is available
3. We will publish a security advisory on GitHub
4. The vulnerability will be documented in release notes
5. A new version will be published to NuGet

## Contact

For any security-related questions or concerns, please:

- Open a [GitHub Security Advisory](https://github.com/LabEG/needle-crud/security/advisories/new)
- Email: labeg@mail.ru (subject: "NeedleCrud Security")
- Create an issue at: <https://github.com/LabEG/needle-crud/issues> (only for non-sensitive security discussions)

---

Thank you for helping keep NeedleCrud and its users safe!
