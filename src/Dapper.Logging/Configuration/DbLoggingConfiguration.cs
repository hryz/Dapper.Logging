using Microsoft.Extensions.Logging;

namespace Dapper.Logging.Configuration
{
    public class DbLoggingConfiguration
    {
        public DbLoggingConfiguration(
            LogLevel logLevel,
            string openConnectionMessage,
            string closeConnectionMessage,
            string executeQueryMessage)
        {
            LogLevel = logLevel;
            OpenConnectionMessage = openConnectionMessage;
            CloseConnectionMessage = closeConnectionMessage;
            ExecuteQueryMessage = executeQueryMessage;
        }

        public LogLevel LogLevel { get; }
        public string OpenConnectionMessage { get; }
        public string CloseConnectionMessage { get; }
        public string ExecuteQueryMessage { get; }
    }
}