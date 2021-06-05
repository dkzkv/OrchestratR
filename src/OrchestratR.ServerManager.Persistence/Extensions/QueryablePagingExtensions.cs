using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrchestratR.Core.Paging;

namespace OrchestratR.ServerManager.Persistence.Extensions
{
    public static class QueryablePagingExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> query, IPageFilter pageFilter)
        {
            return query
                .Skip(pageFilter.Offset)
                .Take(pageFilter.Count);
        }

        public static async Task<Page<T>> Page<T>(this IOrderedQueryable<T> source, IPageFilter filter, CancellationToken cancellationToken = default)
        {
            var total = await source.CountAsync(cancellationToken);
            var items = await source
                .Skip(filter.Offset)
                .Take(filter.Count)
                .ToListAsync(cancellationToken);

            return new Page<T>(items, total);
        }

        public static async Task<Page<TResult>> Page<TSource, TResult>(this IOrderedQueryable<TSource> source, IPageFilter filter, Func<TSource, TResult> map,
            CancellationToken cancellationToken = default)
        {
            var total = await source.CountAsync(cancellationToken);
            var items = await source
                .Skip(filter.Offset)
                .Take(filter.Count)
                .ToListAsync(cancellationToken);

            return new Page<TResult>(items.Select(map), total);
        }
    }
}