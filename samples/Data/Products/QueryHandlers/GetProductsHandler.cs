using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Dapper.Logging;
using Data.Abstract;
using Data.Products.Queries;
using Data.Products.ReadModels;
using Microsoft.Extensions.Logging;

namespace Data.Products.QueryHandlers
{
    public class GetProductsHandler : IPageHandler<GetProductList, Product>
    {
        private const string CountQuery = @"
        SELECT count(*)
        FROM Production.Product p
        WHERE p.ListPrice > 0";

        private const string PageQuery = @"
        SELECT
          ProductID     AS Id,
          Name          AS Name,
          ProductNumber AS Code,
          ListPrice     AS Price
        FROM Production.Product p
        WHERE p.ListPrice > 0
        ORDER BY ProductID
          OFFSET @skip ROWS
          FETCH NEXT @take ROWS ONLY";

        private readonly IConnectionString _connectionString;
        private readonly ILogger<DbCommand> _logger;

        public GetProductsHandler(IConnectionString connectionString, ILogger<DbCommand> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<IPageResult<Product>> Handle(GetProductList request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                skip = request.Skip(),
                take = request.Take()
            };

            using (var db = new SqlConnection(_connectionString.Value).WithLog(_logger))
            {
                var count = await db.ExecuteScalarAsync<int>(CountQuery);
                var page = await db.QueryAsync<Product>(PageQuery, parameters);
                return new PageResult<Product>(count, page);
            }
        }
    }
}
