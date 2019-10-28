using System;
using System.Threading.Tasks;

namespace Mimir.CQRS.Queries
{
    public class QueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult> DispatchAsync<TResult, TQuery>(TQuery query) where TQuery : IQuery
        {
            var queryHandler = _serviceProvider.GetService(typeof(IQueryHandler<TResult, TQuery>)) as IQueryHandler<TResult, TQuery>;
            if (queryHandler != null)
            {
                return await queryHandler.HandleAsync(query);
            }
            else
            {
                throw new ArgumentException($"There is no query handler for query type: {typeof(TQuery)} with result types: {typeof(TResult)}");
            }
        }
    }
}
