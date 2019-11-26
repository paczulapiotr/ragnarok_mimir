using System;
using System.Threading.Tasks;

namespace Mimir.CQRS.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(typeof(TResult), query.GetType());
            object queryHandler = _serviceProvider.GetService(handlerType);

            if (queryHandler != null)
            {
                var method = handlerType.GetMethod(nameof(IQueryHandler<TResult, IQuery<TResult>>.HandleAsync));
                var task = method.Invoke(queryHandler, new[] { query }) as Task<TResult>;
                return await task;
            }
            else
            {
                throw new ArgumentException($"There is no query handler for query type: {query.GetType()} with result types: {typeof(TResult)}");
            }
        }
    }
}
