using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mimir.Core.CommonExceptions;
using Mimir.CQRS.Commands;
using Mimir.Database;
using Mimir.Kanban;

namespace Mimir.API.Commands
{
    public class EditBoardCommandHandler : ICommandHandler<EditBoardCommandHandler.Command>
    {
        private readonly MimirDbContext _dbContext;
        private readonly IKanbanAccessService _accessService;

        public EditBoardCommandHandler(MimirDbContext dbContext, IKanbanAccessService accessService)
        {
            _dbContext = dbContext;
            _accessService = accessService;
        }

        public Task HandleAsync(Command command)
        {
            if (!_accessService.IsOwner(command.UserId, command.BoardId))
                throw new ForbiddenException();

            if (string.IsNullOrWhiteSpace(command.Name))
                throw new ArgumentException("Name cannot be empty");

            var board = _dbContext.KanbanBoards.FirstOrDefault(x => x.ID == command.BoardId);
            board.Name = command.Name;
            return _dbContext.SaveChangesAsync();
        }

        public class Command: ICommand
        {
            public int UserId { get; set; }
            public int BoardId { get; set; }
            public string Name { get; set; }
        }
    }
}
