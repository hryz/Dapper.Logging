using System;
using System.Data.Common;

namespace Dapper.Logging.Hooks
{
    internal class WrappedConnectionFactory<T> : IHookedDbConnectionFactory<T>
    {
        private readonly Func<DbConnection> _factory;
        public WrappedConnectionFactory(Func<DbConnection> factory) => _factory = factory;

        public DbConnection CreateConnection(ISqlHooks<T> hooks, T context) => 
            new WrappedConnection<T>(_factory(), hooks, context);
    }
}