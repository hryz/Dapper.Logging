using System;
using System.Data.Common;
using Dapper.Logging.Configuration;
using Microsoft.Extensions.Logging;

namespace Dapper.Logging
{
    internal class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly ILogger<DbConnection> _logger;
        private readonly Func<DbConnection> _factory;
        private readonly DbLoggingConfiguration _cfg;

        public DbConnectionFactory(ILogger<DbConnection> logger, Func<DbConnection> factory, DbLoggingConfiguration cfg)
        {
            _logger = logger;
            _factory = factory;
            _cfg = cfg;
        }

        public DbConnection CreateConnection()
        {
            return new LoggedConnection(_factory(), _logger, _cfg);
        }
    }
}