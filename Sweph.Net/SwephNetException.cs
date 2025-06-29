using System.Globalization;

namespace Sweph.Net;

/// <summary>
/// Represents an error that occurs in the Sweph.Net library. Implements the <see cref="Exception"/>.
/// </summary>
/// <seealso cref="Exception"/>
public class SwephNetException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SwephNetException"/> class.
    /// </summary>
    public SwephNetException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SwephNetException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public SwephNetException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SwephNetException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception, or a null reference ( <see
    /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
    /// </param>
    public SwephNetException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SwephNetException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="args">The arguments.</param>
    public SwephNetException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SwephNetException"/> class.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    /// <param name="message">The message.</param>
    /// <param name="args">The arguments.</param>
    public SwephNetException(Exception innerException, string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args), innerException)
    {
    }
}
