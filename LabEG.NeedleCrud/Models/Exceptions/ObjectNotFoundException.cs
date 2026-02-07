namespace LabEG.NeedleCrud.Models.Exceptions;

/// <summary>
/// Exception thrown when a requested object is not found in the database.
/// In error handlers, this exception should be treated as an HTTP 404 Not Found error.
/// </summary>
public class ObjectNotFoundNeedleCrudException : NeedleCrudException
{
    public override string Message => base.Message is not null ? base.Message : "Object Not Found";
}
