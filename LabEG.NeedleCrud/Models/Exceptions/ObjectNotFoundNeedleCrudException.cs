namespace LabEG.NeedleCrud.Models.Exceptions;

/// <summary>
/// Exception thrown when a requested object is not found in the database.
/// In error handlers, this exception should be treated as an HTTP 404 Not Found error.
/// </summary>
public class ObjectNotFoundNeedleCrudException : NeedleCrudException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectNotFoundNeedleCrudException"/> class
    /// with a default "Object Not Found" message.
    /// </summary>
    public ObjectNotFoundNeedleCrudException() : base("Object Not Found")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectNotFoundNeedleCrudException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ObjectNotFoundNeedleCrudException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectNotFoundNeedleCrudException"/> class
    /// with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ObjectNotFoundNeedleCrudException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
