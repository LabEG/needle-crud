namespace LabEG.NeedleCrud.Models.Entities;

/// <summary>
/// Base interface for entities with a primary key
/// </summary>
/// <typeparam name="T">Type of the primary key</typeparam>
public interface IEntity<T>
{
    /// <summary>
    /// Primary key identifier of the entity
    /// </summary>
    T? Id { get; set; }
}