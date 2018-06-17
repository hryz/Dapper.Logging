using MediatR;

namespace Data.Abstract
{
    public interface IPageHandler<in TQuery, TResult> : 
        IRequestHandler<TQuery, IPageResult<TResult>>
        where TQuery : IPageQuery<TResult>
    {
    }
}