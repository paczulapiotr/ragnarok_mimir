﻿using System.Threading.Tasks;

namespace Mimir.CQRS.Commands
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
