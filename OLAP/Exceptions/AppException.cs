using Microsoft.AspNetCore.Identity;

namespace OLAP.API.Exceptions
{
    //
    // Summary:
    //     Represents errors that occur during application execution.
    public class AppException : Exception
    {
        // Summary:
        //     Initializes a new instance of the System.Exception class.
        public AppException() : base()
        { }

        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with a specified error
        //     message.
        //
        // Parameters:
        //   message:
        //     The message that describes the error.
        public AppException(string? message) : base(message)
        { }

        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with a specified error
        //     message and a reference to the inner exception that is the cause of this exception.
        //
        // Parameters:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception, or a null reference
        //     (Nothing in Visual Basic) if no inner exception is specified.
        public AppException(string? message, Exception? innerException) : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the System.Exception class.
        /// </summary>
        /// <param name="errors"></param>
        public AppException(IEnumerable<IdentityError> errors) : base(string.Join(Environment.NewLine, errors.Select(x => $"{x.Code} {x.Description}")))
        {
        }
    }
}
