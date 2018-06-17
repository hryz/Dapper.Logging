using System.Data.Common;

namespace Dapper.Logging
{
    public interface IDbConnectionFactory
    {
        DbConnection CreateConnection();
    }
}
