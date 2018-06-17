using Microsoft.Extensions.Logging;

namespace Dapper.Logging.Configuration
{
    public class DbLoggingConfigurationBuilder
    {
        private const string DefaultQueryMessage = "Dapper query:\r\n{0}, elapsed: {1} ms";

        private static readonly DbLoggingConfiguration DefaultConfig = new DbLoggingConfiguration(
            LogLevel.Information,
            "Dapper connection: open, elapsed: {0} ms",
            "Dapper connection: open async, elapsed: {0} ms",
            "Dapper connection: close, elapsed: {0} ms",
            DefaultQueryMessage,
            DefaultQueryMessage,
            DefaultQueryMessage,
            DefaultQueryMessage,
            DefaultQueryMessage,
            DefaultQueryMessage);

        public LogLevel? LogLevelValue { get; set; }

        public string OpenConnectionMessage { get; set; }
        public string OpenConnectionAsyncMessage { get; set; }
        public string CloseConnectionMessage { get; set; }

        public string ExecuteNonQueryMessage { get; set; }
        public string ExecuteNonQueryAsyncMessage { get; set; }

        public string ExecuteScalarMessage { get; set; }
        public string ExecuteScalarAsyncMessage { get; set; }

        public string ExecuteReaderMessage { get; set; }
        public string ExecuteReaderAsyncMessage { get; set; }

        public DbLoggingConfiguration Build()
        {
            return new DbLoggingConfiguration(
                LogLevelValue ?? DefaultConfig.LogLevel,
                OpenConnectionMessage ?? DefaultConfig.OpenConnectionMessage,
                OpenConnectionAsyncMessage ?? DefaultConfig.OpenConnectionAsyncMessage,
                CloseConnectionMessage ?? DefaultConfig.CloseConnectionMessage,
                ExecuteNonQueryMessage ?? DefaultConfig.ExecuteNonQueryMessage,
                ExecuteNonQueryAsyncMessage ?? DefaultConfig.ExecuteNonQueryAsyncMessage,
                ExecuteScalarMessage ?? DefaultConfig.ExecuteScalarMessage,
                ExecuteScalarAsyncMessage ?? DefaultConfig.ExecuteScalarAsyncMessage,
                ExecuteReaderMessage ?? DefaultConfig.ExecuteReaderMessage,
                ExecuteReaderAsyncMessage ?? DefaultConfig.ExecuteReaderAsyncMessage);
        }

        public static implicit operator DbLoggingConfiguration(DbLoggingConfigurationBuilder src)
        {
            return src.Build();
        }
    }
}