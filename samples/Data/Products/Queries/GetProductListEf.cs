using Data.Abstract;
using Data.Products.ReadModels;

namespace Data.Products.Queries
{
    public class GetProductListEf : IPageQuery<ProductEf>
    {
        public GetProductListEf(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; }
        public int PageSize { get; }
    }
}
