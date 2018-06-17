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
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<IPageResult<Product>> GetList(int page, int pageSize, CancellationToken token)
        {
            return _mediator.Send(new GetProductList(page, pageSize), token);
        }
    }
}
