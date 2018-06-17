using Data.Abstract;
using Data.Products.ReadModels;

namespace Data.Products.Queries
{
    public class GetProductList : IPageQuery<Product>
    {
        public GetProductList(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; }
        public int PageSize { get; }
    }
}
