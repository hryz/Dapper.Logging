using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Dapper.Logging;
using Data.Abstract;
using Data.Products.Queries;
using Data.Products.ReadModels;

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


        private readonly IDbConnectionFactory _connectionFactory;

        public GetProductsHandler(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IPageResult<Product>> Handle(GetProductList request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                skip = request.Skip(),
                take = request.Take()
            };

            using (var db = _connectionFactory.CreateConnection())
            {
                var count = await db.ExecuteScalarAsync<int>(CountQuery);
                var page = await db.QueryAsync<Product>(PageQuery, parameters);
                return new PageResult<Product>(count, page);
            }
        }
    }
}
