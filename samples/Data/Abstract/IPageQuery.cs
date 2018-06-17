using MediatR;

namespace Data.Abstract
{
    public interface IPageQuery<out T> : IRequest<IPageResult<T>>
    {
        int Page { get; }
        int PageSize { get; }
    }
}