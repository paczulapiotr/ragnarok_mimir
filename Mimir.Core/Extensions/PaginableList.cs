using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mimir.Core.Extensions
{
    public class PaginableList<T>
    {
        public PaginableList()
        {
        }

        public PaginableList(IEnumerable<T> list) 
        {
            List = list.ToList();
        }

        public PaginableList(IEnumerable<T> list, int page, int pageCount, int totalCount)
        {
            List = list.ToList();
            Page = page;
            PageCount = pageCount;
            TotalCount = totalCount;
        }

        public List<T> List { get; set; }
        public int Page { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
    }

    public static class PaginableListExtensions
    {
        public static PaginableList<T> ToPaginableList<T>(this IEnumerable<T> @this)
            => new PaginableList<T>(@this);

        public static Task<PaginableList<T>> ToPaginableListAsync<T>(this IEnumerable<T> @this)
            => Task.FromResult(new PaginableList<T>(@this));

        public static PaginableList<T> ToPaginableList<T>(this IEnumerable<T> @this, int page, int pageCount, int totalCount)
            => new PaginableList<T>(@this, page, pageCount, totalCount);

        public static Task<PaginableList<T>> ToPaginableListAsync<T>(this IEnumerable<T> @this, int page, int pageCount, int totalCount)
             => Task.FromResult(new PaginableList<T>(@this, page, pageCount, totalCount));
    }
}
