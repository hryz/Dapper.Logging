using System.Collections.Generic;

namespace Data.Abstract
{
    public interface IPageResult<out T>
    {
        int TotalCount { get; }
        IEnumerable<T> Page { get; }
    }
}