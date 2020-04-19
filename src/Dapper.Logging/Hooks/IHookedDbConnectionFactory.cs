using System.Data.Common;

namespace Dapper.Logging.Hooks
{
    public interface IHookedDbConnectionFactory<T>
    {
        DbConnection CreateConnection(ISqlHooks<T> hooks, T context);
    }
}
