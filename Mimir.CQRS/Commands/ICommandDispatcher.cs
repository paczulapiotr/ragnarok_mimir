using System.Threading.Tasks;

namespace Mimir.CQRS.Commands
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
