using System.Collections.Generic;

namespace Data.Abstract
{
    public class PageResult<T> : IPageResult<T>
    {
        public PageResult(int totalCount, IEnumerable<T> page)
        {
            TotalCount = totalCount;
            Page = page;
        }

        public int TotalCount { get; }
        public IEnumerable<T> Page { get; }
    }
}