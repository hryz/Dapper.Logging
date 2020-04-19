using System;
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
        FROM public.product p
        WHERE p.price > 0";

        private const string PageQuery = @"
        SELECT id      AS Id,
               name    AS Name,
               code    AS Code,
               price   AS Price,
               deleted AS Deleted
        FROM public.product p
        WHERE p.price > 0
        ORDER BY id
            OFFSET :skip
            LIMIT :take";


        private readonly IDbConnectionFactory<LoggingContext> _connectionFactory;

        public GetProductsHandler(IDbConnectionFactory<LoggingContext> connectionFactory)
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

            var ctx = new LoggingContext
            {
                ConnectionId = Guid.NewGuid().ToString("N").Substring(0,8),
                UserId = "j.doe@gmail.com"
            };

            using (var db = _connectionFactory.CreateConnection(ctx))
            {
                 var count = await db.ExecuteScalarAsync<int>(CountQuery);
                 var page = await db.QueryAsync<Product>(PageQuery, parameters);
                 return new PageResult<Product>(count, page);
            }
        }
    }
}
