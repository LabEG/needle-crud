namespace LabEG.NeedleCrud.Models.ViewModels;

/// <summary>
/// Contains the result of a GetAll operation together with the true total count,
/// allowing callers (e.g. the controller) to detect whether the response was truncated
/// and emit the appropriate <c>X-Total-Count</c> / <c>Warning</c> HTTP headers.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public record GetAllResult<TEntity>(TEntity[] Items, int TotalCount)
{
    /// <summary>
    /// Returns <see langword="true"/> when the result was truncated,
    /// i.e. <see cref="Items"/> contains fewer elements than <see cref="TotalCount"/>.
    /// </summary>
    public bool IsTruncated => Items.Length < TotalCount;
}
