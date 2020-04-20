using System;
using System.Data.Common;
using Dapper.Logging.Configuration;
using Dapper.Logging.Hooks;
using Microsoft.Extensions.Logging;

namespace Dapper.Logging
{
    public class ContextfulLoggingFactory<T> : IDbConnectionFactory<T>
    {
        private readonly LoggingHook<T> _hooks;
        private readonly WrappedConnectionFactory<T> _factory;

        public ContextfulLoggingFactory(
            ILogger<DbConnection> logger, 
            DbLoggingConfiguration config, 
            Func<DbConnection> factory)
        {
            _hooks = new LoggingHook<T>(logger, config);
            _factory = new WrappedConnectionFactory<T>(factory);
        }

        public DbConnection CreateConnection(T context) => 
            _factory.CreateConnection(_hooks, context);
    }
}