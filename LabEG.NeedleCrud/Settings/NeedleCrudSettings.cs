namespace LabEG.NeedleCrud.Settings;

/// <summary>
/// Configuration settings for the NeedleCrud library.
/// </summary>
/// <remarks>
/// <para>
/// These settings control the upper bounds for various operations exposed by
/// <see cref="Controllers.CrudController{TEntity,TId}"/> and the underlying repository layer,
/// protecting the application against resource exhaustion caused by unbounded client requests.
/// </para>
/// <para>
/// Register the settings through the ASP.NET Core options system:
/// <code>
/// builder.Services.Configure&lt;NeedleCrudSettings&gt;(
///     builder.Configuration.GetSection("NeedleCrud"));
/// </code>
/// or configure them directly in code:
/// <code>
/// builder.Services.Configure&lt;NeedleCrudSettings&gt;(options =>
/// {
///     options.MaxGetAllCount  = 500;
///     options.MaxPageSize     = 50;
///     options.MaxFilterCount  = 20;
///     options.MaxSortCount    = 10;
///     options.MaxGraphDepth   = 8;
/// });
/// </code>
/// </para>
/// <para>
/// Corresponding <c>appsettings.json</c> section:
/// <code>
/// {
///   "NeedleCrud": {
///     "MaxGetAllCount":  1000,
///     "MaxPageSize":     100,
///     "MaxFilterCount":  50,
///     "MaxSortCount":    50,
///     "MaxGraphDepth":   16
///   }
/// }
/// </code>
/// </para>
/// </remarks>
public class NeedleCrudSettings
{
    /// <summary>
    /// Gets or sets the maximum number of entities that may be returned by a single
    /// <c>GET /</c> (GetAll) request.
    /// </summary>
    /// <value>
    /// A positive integer specifying the hard upper limit on the result set size.
    /// Defaults to <c>1000</c>.
    /// </value>
    /// <remarks>
    /// When the total number of rows in the table exceeds this value the response will
    /// be silently truncated to <see cref="MaxGetAllCount"/> elements.
    /// For large datasets prefer the paginated endpoint (<c>GET /paged</c>) which
    /// returns a bounded page together with cursor metadata.
    /// </remarks>
    public int MaxGetAllCount { get; set; } = 1000;

    /// <summary>
    /// Gets or sets the maximum allowed value for the <c>pageSize</c> query parameter
    /// of the <c>GET /paged</c> endpoint.
    /// </summary>
    /// <value>
    /// A positive integer. Defaults to <c>100</c>.
    /// </value>
    /// <remarks>
    /// If a client requests a page size larger than this value the request is rejected
    /// with a validation error, preventing excessively large database reads in a single
    /// round-trip.
    /// </remarks>
    public int MaxPageSize { get; set; } = 100;

    /// <summary>
    /// Gets or sets the maximum number of filter conditions that may be included in a
    /// single <c>GET /paged</c> request via the <c>filter</c> query parameter.
    /// </summary>
    /// <value>
    /// A positive integer. Defaults to <c>50</c>.
    /// </value>
    /// <remarks>
    /// Each filter condition is translated into a SQL <c>WHERE</c> predicate. Limiting
    /// the count prevents query-complexity attacks that could cause the database engine
    /// to spend excessive time building or executing the query plan.
    /// </remarks>
    public int MaxFilterCount { get; set; } = 50;

    /// <summary>
    /// Gets or sets the maximum number of sort conditions that may be included in a
    /// single <c>GET /paged</c> request via the <c>sort</c> query parameter.
    /// </summary>
    /// <value>
    /// A positive integer. Defaults to <c>50</c>.
    /// </value>
    /// <remarks>
    /// Each sort condition maps to an <c>ORDER BY</c> clause column. Capping the count
    /// avoids generating pathological <c>ORDER BY</c> expressions that are expensive for
    /// the query planner and rarely useful in practice beyond a handful of columns.
    /// </remarks>
    public int MaxSortCount { get; set; } = 50;

    /// <summary>
    /// Gets or sets the maximum nesting depth of the <c>graph</c> JSON object accepted
    /// by the <c>GET /paged</c> and <c>GET /{id}/graph</c> endpoints.
    /// </summary>
    /// <value>
    /// A positive integer. Defaults to <c>16</c>.
    /// </value>
    /// <remarks>
    /// The graph parameter drives EF Core <c>Include</c> / <c>ThenInclude</c> chains.
    /// Limiting the depth prevents clients from triggering deep recursive eager-loading
    /// chains that would produce enormously wide SQL <c>JOIN</c> trees and potentially
    /// unbounded memory consumption on the server. This also serves as a practical
    /// limit on the total number of relations that can be included in a single query.
    /// </remarks>
    public int MaxGraphDepth { get; set; } = 16;

    /// <summary>
    /// Gets or sets a value indicating whether the repository layer acts as its own
    /// Unit of Work by automatically calling <c>SaveChangesAsync</c> after each
    /// write operation (Create, Update, Delete).
    /// </summary>
    /// <value>
    /// <c>true</c> — the repository calls <c>SaveChangesAsync</c> internally after
    /// every write, and <see cref="Services.CrudDbService{TDbContext,TEntity,TId}"/>
    /// skips the redundant save call.
    /// <para/>
    /// <c>false</c> (default) — the repository only stages changes in the
    /// <see cref="Microsoft.EntityFrameworkCore.DbContext"/> change tracker;
    /// <see cref="Services.CrudDbService{TDbContext,TEntity,TId}"/> is responsible
    /// for committing via <c>SaveChangesAsync</c>. This is also the mode to choose
    /// when you manage the transaction boundary yourself (e.g. batching multiple
    /// service calls before a single commit).
    /// </value>
    /// <remarks>
    /// Set to <c>true</c> when you use <c>CrudDbRepository</c> directly — without
    /// going through <c>CrudDbService</c> — and want each operation to be persisted
    /// immediately. When using the full service stack the default (<c>false</c>) is
    /// recommended because it gives you control over transaction granularity.
    /// </remarks>
    public bool UnitOfWork { get; set; } = false;
}