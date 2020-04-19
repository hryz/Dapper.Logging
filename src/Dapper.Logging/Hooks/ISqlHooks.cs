using System;
using System.Data.Common;

namespace Dapper.Logging.Hooks
{
    public interface ISqlHooks<in T>
    {
        void ConnectionOpened(DbConnection connection, T context, long elapsedMs);
        void ConnectionClosed(DbConnection connection, T context, long elapsedMs);
        void CommandExecuted(DbCommand command, T context, long elapsedMs);
    }
}