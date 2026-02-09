using Microsoft.AspNetCore.Builder;

namespace LabEG.NeedleCrud.Infrastructure;

/// <summary>
/// Extension methods for adding NeedleCrud middleware to the application pipeline.
/// </summary>
public static class NeedleCrudMiddlewareExtensions
{
    /// <summary>
    /// Adds the NeedleCrud exception handler middleware to the application pipeline.
    /// This middleware catches <see cref="Models.Exceptions.NeedleCrudException"/> and
    /// <see cref="Models.Exceptions.ObjectNotFoundNeedleCrudException"/> exceptions
    /// and converts them to appropriate HTTP responses (400 and 404).
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder for chaining.</returns>
    public static IApplicationBuilder UseNeedleCrudExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<NeedleCrudExceptionHandlerMiddleware>();
    }
}
