using Level = Microsoft.Extensions.Logging.LogLevel;
using static System.Environment;

namespace Dapper.Logging.Configuration
{
    public class DbLoggingConfigurationBuilder
    {
        private static readonly DbLoggingConfiguration Default = new DbLoggingConfiguration(
            logLevel: Level.Information,
            openConnectionMessage: "Dapper connection: open, elapsed: {elapsed} ms, context: {@context}",
            closeConnectionMessage: "Dapper connection: close, elapsed: {elapsed} ms, context: {@context}",
            executeQueryMessage: $"Dapper query:{NewLine}{{query}}{NewLine}" +
                                 "Parameters: {params}, elapsed: {elapsed} ms, context: {@context}",
            logSensitiveData: false);

        public Level? LogLevel { get; set; }
        public string OpenConnectionMessage { get; set; }
        public string CloseConnectionMessage { get; set; }
        public string ExecuteQueryMessage { get; set; }
        public bool? LogSensitiveData { get; set; }

        public DbLoggingConfiguration Build()
        {
            return new DbLoggingConfiguration(
                LogLevel ?? Default.LogLevel,
                OpenConnectionMessage ?? Default.OpenConnectionMessage,
                CloseConnectionMessage ?? Default.CloseConnectionMessage,
                ExecuteQueryMessage ?? Default.ExecuteQueryMessage,
                LogSensitiveData ?? Default.LogSensitiveData);
        }

        public static implicit operator DbLoggingConfiguration(DbLoggingConfigurationBuilder src)
        {
            return src.Build();
        }
    }
}