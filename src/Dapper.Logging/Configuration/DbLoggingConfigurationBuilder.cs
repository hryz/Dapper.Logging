using Level = Microsoft.Extensions.Logging.LogLevel;

namespace Dapper.Logging.Configuration
{
    public class DbLoggingConfigurationBuilder
    {
        private static readonly DbLoggingConfiguration Default = new DbLoggingConfiguration(
            Level.Information,
            "Dapper connection: open, elapsed: {0} ms",
            "Dapper connection: close, elapsed: {0} ms",
            "Dapper query:\r\n{0}, elapsed: {1} ms");

        public Level? LogLevel { get; set; }
        public string OpenConnectionMessage { get; set; }
        public string CloseConnectionMessage { get; set; }
        public string ExecuteQueryMessage { get; set; }

        public DbLoggingConfiguration Build()
        {
            return new DbLoggingConfiguration(
                LogLevel ?? Default.LogLevel,
                OpenConnectionMessage ?? Default.OpenConnectionMessage,
                CloseConnectionMessage ?? Default.CloseConnectionMessage,
                ExecuteQueryMessage ?? Default.ExecuteQueryMessage);
        }

        public static implicit operator DbLoggingConfiguration(DbLoggingConfigurationBuilder src)
        {
            return src.Build();
        }
    }
}