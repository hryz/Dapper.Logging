using System;
using System.Data.Common;
using System.Diagnostics;
using Dapper.Logging.Configuration;
using Dapper.Logging.Hooks;
using Microsoft.Extensions.Logging;

namespace Dapper.Logging
{
    internal class LoggingHook<T> : ISqlHooks<T>
    {
        private readonly ILogger _logger;
        private readonly DbLoggingConfiguration _config;

        public LoggingHook(ILogger logger, DbLoggingConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public void ConnectionOpened(DbConnection connection, T context, long elapsedMs) => 
            _logger.Log(
                _config.LogLevel, 
                _config.OpenConnectionMessage,
                elapsedMs,
                context);

        public void ConnectionClosed(DbConnection connection, T context, long elapsedMs) => 
            _logger.Log(
                _config.LogLevel, 
                _config.CloseConnectionMessage, 
                elapsedMs,
                context);

        public void CommandExecuted(DbCommand command, T context, long elapsedMs) =>
            _logger.Log(
                _config.LogLevel, 
                _config.ExecuteQueryMessage, 
                command.CommandText,
                command.GetParameters(hideValues: !_config.LogSensitiveData),
                elapsedMs,
                context);
    }
}