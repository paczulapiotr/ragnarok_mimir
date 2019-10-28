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

        public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(typeof(TResult), query.GetType());
            dynamic queryHandler = _serviceProvider.GetService(handlerType);

            if (queryHandler != null)
            {
                return await queryHandler.HandleAsync(query);
            }
            else
            {
                throw new ArgumentException($"There is no query handler for query type: {query.GetType()} with result types: {typeof(TResult)}");
            }
        }
    }
}
