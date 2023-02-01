using Microsoft.EntityFrameworkCore;

namespace Ecssr.Demo.Application.Common.Pagination
{
    /// <summary>
    /// This class return the paged result with pagination result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }

    /// <summary>
    /// Extension class to get the result based on pagination details
    /// </summary>
    public static class PaginationResult
    {
        public static async Task<PagedResult<T>> FetchByPaginationAsync<T>(this IQueryable<T> query,
                                        int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }
    }
}
