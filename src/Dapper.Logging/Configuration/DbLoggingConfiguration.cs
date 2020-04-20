using System;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using static System.Environment;

namespace Dapper.Logging.Configuration
{
    public class DbLoggingConfiguration
    {
        public static readonly DbLoggingConfiguration Default = new DbLoggingConfiguration(
            logLevel: LogLevel.Information,
            openConnectionMessage: "Dapper connection: open, elapsed: {elapsed} ms, " +
                                   "context: {@context}, connection: {@connection}",
            closeConnectionMessage: "Dapper connection: close, elapsed: {elapsed} ms, " +
                                    "context: {@context}, connection: {@connection}",
            executeQueryMessage: $"Dapper query:{NewLine}{{query}}{NewLine}" +
                                 "Parameters: {params}, elapsed: {elapsed} ms, " +
                                 "context: {@context}, connection: {@connection}",
            logSensitiveData: false,
            connectionProjector: _ => Empty.Object);
        
        public DbLoggingConfiguration(
            LogLevel logLevel,
            string openConnectionMessage,
            string closeConnectionMessage,
            string executeQueryMessage, 
            bool logSensitiveData = false,
            Func<DbConnection, object> connectionProjector = null)
        {
            LogLevel = logLevel;
            LogSensitiveData = logSensitiveData;
            OpenConnectionMessage = openConnectionMessage ?? Default.OpenConnectionMessage;
            CloseConnectionMessage = closeConnectionMessage ?? Default.CloseConnectionMessage;
            ExecuteQueryMessage = executeQueryMessage ?? Default.ExecuteQueryMessage;
            ConnectionProjector =  connectionProjector ?? Default.ConnectionProjector;
        }

        public LogLevel LogLevel { get; }
        public string OpenConnectionMessage { get; }
        public string CloseConnectionMessage { get; }
        public string ExecuteQueryMessage { get; }
        public bool LogSensitiveData { get; }
        public Func<DbConnection, object> ConnectionProjector { get; }
    }
}