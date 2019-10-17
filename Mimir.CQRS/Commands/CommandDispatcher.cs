using System;
using System.Threading.Tasks;

namespace Mimir.CQRS.Commands
{
    public class CommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ICommandResult> DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            var commandHandler = _serviceProvider.GetService(typeof(ICommandHandler<TCommand>)) as ICommandHandler<TCommand>;
            if (commandHandler != null)
            {
                return await commandHandler.HandleAsync(command);
            }
            else
            {
                throw new ArgumentException($"There is no command handler for command type: {typeof(TCommand)}");
            }
        }
    }
}
