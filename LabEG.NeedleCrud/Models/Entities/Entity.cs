namespace LabEG.NeedleCrud.Models.Entities;

public class Entity<T> : IEntity<T> // T can be Long, String, Guid and others
{
    public T Id { get; set; }

    public DateTime CreatedTime { get; set; }
    public DateTime LastUpdateTime { get; set; }
}