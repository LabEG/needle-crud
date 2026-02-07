
namespace LabEG.NeedleCrud.Models.Exceptions;

/// <summary>
/// Base exception class for the NeedleCrud library.
/// In error handlers, this exception should be treated as an HTTP 400 Bad Request error.
/// </summary>
public class NeedleCrudException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NeedleCrudException"/> class.
    /// </summary>
    public NeedleCrudException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NeedleCrudException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NeedleCrudException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NeedleCrudException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public NeedleCrudException(string message, Exception innerException) : base(message, innerException)
    {
    }
}