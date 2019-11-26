using System.Threading.Tasks;

namespace Mimir.CQRS.Queries
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query);
    }
}
