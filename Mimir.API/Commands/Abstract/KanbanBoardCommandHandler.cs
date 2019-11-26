using Mimir.Core.CommonExceptions;
using Mimir.CQRS.Commands;
using Mimir.Kanban;
using System.Threading.Tasks;

namespace Mimir.API.Commands.Abstract
{
    public abstract class KanbanBoardCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : KanbanBoardCommand
    {
        protected readonly IKanbanRepository _repository;
        protected readonly IKanbanAccessService _accessService;

        public KanbanBoardCommandHandler(IKanbanRepository repository, IKanbanAccessService accessService)
        {
            _repository = repository;
            _accessService = accessService;
        }

        public virtual Task HandleAsync(TCommand command)
        {
            var hasAccess = _accessService.HasAccess(command.UserId, command.BoardId);
            if (!hasAccess)
                throw new ForbiddenException($"This action is forbidden for user {command.UserId}");

            return Task.CompletedTask;
        }

    }
}
