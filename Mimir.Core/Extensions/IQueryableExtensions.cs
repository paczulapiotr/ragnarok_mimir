using System;
using System.Linq;

namespace Mimir.Core.Extensions
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page">Starting from page 1</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> @this, int page = 1, int pageSize = 5)
            => page < 1 
            ? throw new ArgumentException("Page number cannot be lower than 1") 
            : @this.Skip((page - 1) * pageSize).Take(pageSize);
    }
}
