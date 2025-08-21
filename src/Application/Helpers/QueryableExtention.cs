using System.Linq.Expressions;

namespace Application.Helpers;
public static class QueryableExtention
{
  

    public static IQueryable<TSource> AddPage<TSource>(this IQueryable<TSource> source, int? skip, int? take)
    {
        if (skip.HasValue)
            source = source.Skip(skip.Value);

        if (take.HasValue)
            source = source.Take(take.Value);
        return source;
    }
}
