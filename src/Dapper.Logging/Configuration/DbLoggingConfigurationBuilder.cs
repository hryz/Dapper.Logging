using System;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using static Dapper.Logging.Configuration.DbLoggingConfiguration;

namespace Dapper.Logging.Configuration
{
    public class DbLoggingConfigurationBuilder
    {
        public LogLevel? LogLevel { get; set; } = Default.LogLevel;
        public string OpenConnectionMessage { get; set; } = Default.OpenConnectionMessage;
        public string CloseConnectionMessage { get; set; } = Default.CloseConnectionMessage;
        public string ExecuteQueryMessage { get; set; } = Default.ExecuteQueryMessage;
        public bool? LogSensitiveData { get; set; } = Default.LogSensitiveData;
        public Func<DbConnection, object> ConnectionProjector { get; set; } = Default.ConnectionProjector;

        public DbLoggingConfiguration Build() =>
            new DbLoggingConfiguration(
                LogLevel ?? Default.LogLevel,
                OpenConnectionMessage,
                CloseConnectionMessage,
                ExecuteQueryMessage,
                LogSensitiveData ?? Default.LogSensitiveData,
                ConnectionProjector);

        public static implicit operator DbLoggingConfiguration(DbLoggingConfigurationBuilder src) =>
            src.Build();
    }
}