using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mimir.API.Commands;
using Mimir.API.Controllers.Abstract;
using Mimir.API.DTO;
using Mimir.API.Queries;
using Mimir.CQRS.Commands;
using Mimir.CQRS.Queries;
using System.Threading.Tasks;

namespace Mimir.API.Controllers
{
    public class KanbanController : MimirController
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly QueryDispatcher _queryHandler;

        public KanbanController(IUserResolver userResolver, CommandDispatcher commandDispatcher, QueryDispatcher queryHandler) 
            : base(userResolver)
        {
            _commandDispatcher = commandDispatcher;
            _queryHandler = queryHandler;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> MoveItem([FromBody] KanbanItemMoveDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new MoveKanbanItemCommandHandler.Command(user.ID, dto.BoardId, dto.ItemId, dto.Index, dto.ColumnDestId, dto.Timestamp));

            return await KanbanStateResult(dto.BoardId);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> MoveColumn([FromBody] KanbanColumnMoveDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new MoveKanbanColumnCommandHandler.Command(user.ID, dto.BoardId, dto.ColumnId, dto.Index, dto.Timestamp));

            return await KanbanStateResult(dto.BoardId);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddColumn([FromBody] KanbanColumnAddDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new AddKanbanColumnCommandHandler.Command(user.ID, dto.BoardId, dto.Name, dto.Timestamp));

            return await KanbanStateResult(dto.BoardId);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] KanbanItemAddDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new AddKanbanItemCommandHandler.Command(user.ID, dto.BoardId, dto.Name, dto.ColumnId, dto.Timestamp));

            return await KanbanStateResult(dto.BoardId);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RemoveColumn([FromBody] KanbanColumnRemoveDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new RemoveKanbanColumnCommandHandler.Command(user.ID, dto.BoardId, dto.ColumnId, dto.Timestamp));

            return await KanbanStateResult(dto.BoardId);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RemoveItem([FromBody] KanbanItemRemoveDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new RemoveKanbanItemCommandHandler.Command(user.ID, dto.BoardId, dto.ItemId, dto.Timestamp));

            return await KanbanStateResult(dto.BoardId);
        }

        [AllowAnonymous]
        [HttpGet("{boardId}")]
        public Task<IActionResult> Board(int boardId)
        {
            return KanbanStateResult(boardId);
        }

        private async Task<IActionResult> KanbanStateResult(int boardId)
            => Ok(await _queryHandler.DispatchAsync(new KanbanStateQueryHandler.Query(boardId)));
    }
}
