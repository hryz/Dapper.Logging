using Microsoft.Extensions.Logging;

namespace Dapper.Logging.Configuration
{
    public class DbLoggingConfiguration
    {
        public DbLoggingConfiguration(
            LogLevel logLevel,
            string openConnectionMessage,
            string openConnectionAsyncMessage,
            string closeConnectionMessage,
            string executeNonQueryMessage,
            string executeNonQueryAsyncMessage,
            string executeScalarMessage,
            string executeScalarAsyncMessage,
            string executeReaderMessage,
            string executeReaderAsyncMessage)
        {
            LogLevel = logLevel;
            OpenConnectionMessage = openConnectionMessage;
            OpenConnectionAsyncMessage = openConnectionAsyncMessage;
            CloseConnectionMessage = closeConnectionMessage;
            ExecuteNonQueryMessage = executeNonQueryMessage;
            ExecuteNonQueryAsyncMessage = executeNonQueryAsyncMessage;
            ExecuteScalarMessage = executeScalarMessage;
            ExecuteScalarAsyncMessage = executeScalarAsyncMessage;
            ExecuteReaderMessage = executeReaderMessage;
            ExecuteReaderAsyncMessage = executeReaderAsyncMessage;
        }

        public LogLevel LogLevel { get; }

        public string OpenConnectionMessage { get; }
        public string OpenConnectionAsyncMessage { get; }
        public string CloseConnectionMessage { get; }

        public string ExecuteNonQueryMessage { get; }
        public string ExecuteNonQueryAsyncMessage { get; }

        public string ExecuteScalarMessage { get; }
        public string ExecuteScalarAsyncMessage { get; }

        public string ExecuteReaderMessage { get; }
        public string ExecuteReaderAsyncMessage { get; }
    }
}