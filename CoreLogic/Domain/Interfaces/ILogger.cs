namespace Core_Logic.Domain.Interfaces
{
    /// <summary>
    /// Defines logging methods for informational and error messages.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Log(string message);

        /// <summary>
        /// Logs an error message, optionally with an exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="ex">The exception (optional).</param>
        void LogError(string message, Exception? ex = null);
    }
}