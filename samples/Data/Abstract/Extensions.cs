namespace Data.Abstract
{
    public static class Extensions
    {
        public static int Take<T>(this IPageQuery<T> query) => query.PageSize;
        public static int Skip<T>(this IPageQuery<T> query) => (query.Page - 1) * query.PageSize;
    }
}