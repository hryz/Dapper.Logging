using Microsoft.Extensions.Logging;

namespace Dapper.Logging.Configuration
{
    public static class DbLoggingConfigurationBuilderExtensions
    {
        /// <summary>
        /// Set the log level
        /// </summary>
        /// <param name="x"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static DbLoggingConfigurationBuilder WithLogLevel(this DbLoggingConfigurationBuilder x, LogLevel level)
        {
            x.LogLevelValue = level;
            return x;
        }

        /// <summary>
        /// Sets the Open connection message
        /// </summary>
        /// <param name="x">builder</param>
        /// <param name="message">message template (parameters: elapsed ms)</param>
        /// <returns></returns>
        public static DbLoggingConfigurationBuilder WithOpenConnectionMessage(this DbLoggingConfigurationBuilder x, string message)
        {
            x.OpenConnectionMessage = message;
            return x;
        }

        /// <summary>
        /// Sets the Open connection async message
        /// </summary>
        /// <param name="x">builder</param>
        /// <param name="message">message template (parameters: elapsed ms)</param>
        /// <returns></returns>
        public static DbLoggingConfigurationBuilder WithOpenConnectionAsyncMessage(this DbLoggingConfigurationBuilder x, string message)
        {
            x.OpenConnectionAsyncMessage = message;
            return x;
        }

        /// <summary>
        /// Sets the Close connection message
        /// </summary>
        /// <param name="x">builder</param>
        /// <param name="message">message template (parameters: elapsed ms)</param>
        /// <returns></returns>
        public static DbLoggingConfigurationBuilder WithCloseConnectionMessage(this DbLoggingConfigurationBuilder x, string message)
        {
            x.CloseConnectionMessage = message;
            return x;
        }

        /// <summary>
        /// Sets the message for all query types: Non Query / Scalar / Reader (Sync / Async)
        /// </summary>
        /// <param name="x">builder</param>
        /// <param name="message">message template (parameters: query text, elapsed ms)</param>
        /// <returns></returns>
        public static DbLoggingConfigurationBuilder WithQueryMessage(this DbLoggingConfigurationBuilder x, string message)
        {
            x.ExecuteNonQueryMessage = message;
            x.ExecuteNonQueryAsyncMessage = message;
            x.ExecuteScalarMessage = message;
            x.ExecuteScalarAsyncMessage = message;
            x.ExecuteReaderMessage = message;
            x.ExecuteReaderAsyncMessage = message;
            return x;
        }

        /// <summary>
        /// Sets the Execute Non Query message
        /// </summary>
        /// <param name="x">builder</param>
        /// <param name="message">message template (parameters: query text, elapsed ms)</param>
        /// <returns></returns>
        public static DbLoggingConfigurationBuilder WithExecuteNonQueryMessage(this DbLoggingConfigurationBuilder x, string message)
        {
            x.ExecuteNonQueryMessage = message;
            return x;
        }

        /// <summary>
        /// Sets the Execute Non Query Async message
        /// </summary>
        /// <param name="x">builder</param>
        /// <param name="message">message template (parameters: query text, elapsed ms)</param>
        /// <returns></returns>
        public static DbLoggingConfigurationBuilder WithExecuteNonQueryAsyncMessage(this DbLoggingConfigurationBuilder x, string message)
        {
            x.ExecuteNonQueryAsyncMessage = message;
            return x;
        }

        /// <summary>
        /// Sets the Execute Scalar message
        /// </summary>
        /// <param name="x">builder</param>
        /// <param name="message">message template (parameters: query text, elapsed ms)</param>
        /// <returns></returns>
        public static DbLoggingConfigurationBuilder WithExecuteScalarMessage(this DbLoggingConfigurationBuilder x, string message)
        {
            x.ExecuteScalarMessage = message;
            return x;
        }

        /// <summary>
        /// Sets the Execute Scalar Async message
        /// </summary>
        /// <param name="x">builder</param>
        /// <param name="message">message template (parameters: query text, elapsed ms)</param>
        /// <returns></returns>
        public static DbLoggingConfigurationBuilder WithExecuteScalarAsyncMessage(this DbLoggingConfigurationBuilder x, string message)
        {
            x.ExecuteScalarAsyncMessage = message;
            return x;
        }

        /// <summary>
        /// Sets the Execute Reader message
        /// </summary>
        /// <param name="x">builder</param>
        /// <param name="message">message template (parameters: query text, elapsed ms)</param>
        /// <returns></returns>
        public static DbLoggingConfigurationBuilder WithExecuteReaderMessage(this DbLoggingConfigurationBuilder x, string message)
        {
            x.ExecuteReaderMessage = message;
            return x;
        }

        /// <summary>
        /// Sets the Execute Reader Async message
        /// </summary>
        /// <param name="x">builder</param>
        /// <param name="message">message template (parameters: query text, elapsed ms)</param>
        /// <returns></returns>
        public static DbLoggingConfigurationBuilder WithExecuteReaderAsyncMessage(this DbLoggingConfigurationBuilder x, string message)
        {
            x.ExecuteReaderAsyncMessage = message;
            return x;
        }
    }
}