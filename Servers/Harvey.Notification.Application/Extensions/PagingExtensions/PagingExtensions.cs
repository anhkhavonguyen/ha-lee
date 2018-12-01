using AutoMapper.QueryableExtensions;
using System;
using System.Linq;

namespace Harvey.Notification.Application.Extensions.PagingExtensions
{
    public static class PagingExtensions
    {
        public static PagedResult<U> GetPaged<T, U>(this IQueryable<T> query, int page, int pageSize) where U : class
        {
            var result = new PagedResult<U>();
            result.PageNumber = page;
            result.PageSize = pageSize;
            result.TotalItem = query.Count();

            var pageCount = (double)result.TotalItem / pageSize;

            var skip = page * pageSize;
            result.Results = query.Skip(skip)
                                  .Take(pageSize)
                                  .ProjectTo<U>()
                                  .ToList();

            return result;
        }

        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query,
                                         int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.PageNumber = page;
            result.PageSize = pageSize;
            result.TotalItem = query.Count();


            var pageCount = (double)result.TotalItem / pageSize;

            var skip = page  * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }
}
