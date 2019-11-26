using System;
using Mimir.Core.Extensions;
using Mimir.CQRS.Queries;

namespace Mimir.API.Queries
{
    public class PaginationQuery<T> : IQuery<PaginableList<T>>
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 5;

        public void Validate()
        {
            if (Page < 0)
                throw new ArgumentException("Page must be greater than 0");
            if(PageSize < 0)
                throw new ArgumentException("PageSize must be greater than 0");
        }

        public int GetPageCount(int totalCount)
        {
            return totalCount % PageSize == 0
                ? totalCount / PageSize
                : (totalCount / PageSize) + 1;
        }
    }
}
