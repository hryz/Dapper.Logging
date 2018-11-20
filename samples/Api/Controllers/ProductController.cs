using System.Threading;
using System.Threading.Tasks;
using Data.Abstract;
using Data.Products.Queries;
using Data.Products.ReadModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private const int DefaultPage = 1;
        private const int DefaultSize = 10;

        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list")]
        public Task<IPageResult<Product>> GetList(
            int page = DefaultPage, 
            int pageSize = DefaultSize, 
            CancellationToken token = default)
        {
            return _mediator.Send(new GetProductList(page, pageSize), token);
        }

        [HttpGet("listEf")]
        public Task<IPageResult<ProductEf>> GetListEf(
            int page = DefaultPage, 
            int pageSize = DefaultSize, 
            CancellationToken token = default)
        {
            return _mediator.Send(new GetProductListEf(page, pageSize), token);
        }
    }
}
