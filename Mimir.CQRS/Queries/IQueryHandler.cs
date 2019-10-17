using System.Threading.Tasks;

namespace Mimir.CQRS.Queries
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
