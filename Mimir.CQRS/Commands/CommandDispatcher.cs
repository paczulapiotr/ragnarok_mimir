using System;
using System.Threading.Tasks;

namespace Mimir.CQRS.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            var commandHandler = _serviceProvider.GetService(typeof(ICommandHandler<TCommand>)) as ICommandHandler<TCommand>;
            if (commandHandler != null)
            {
                await commandHandler.HandleAsync(command);
            }
            else
            {
                throw new ArgumentException($"There is no command handler for command type: {typeof(TCommand)}");
            }
        }
    }
}
