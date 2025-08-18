using Core_Logic.Domain.Interfaces;

namespace Core_Logic.Application.Services
{
    /// <summary>
    /// Provides simple thread-safe logging to a file.
    /// </summary>
    public class Logger : ILogger
    {
        private static readonly string LogFilePath =
            Path.Combine(AppContext.BaseDirectory, "Logs", $"log_{DateTime.Now:yyyyMMdd}.txt");
        private static readonly object _lock = new();

        // Private constructor to prevent external instantiation
        public Logger()
        {
            var logDir = Path.GetDirectoryName(LogFilePath);
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir!);
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(string message)
        {
            Write("INFO", message);
        }

        /// <summary>
        /// Logs an error message, optionally with an exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="ex">The exception (optional).</param>
        public void LogError(string message, Exception? ex = null)
        {
            Write("ERROR", $"{message}{(ex != null ? $": {ex}" : string.Empty)}");
        }

        /// <summary>
        /// Writes a log entry to the log file.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The log message.</param>
        private static void Write(string level, string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}";
            lock (_lock)
            {
                File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
            }
        }
    }
}