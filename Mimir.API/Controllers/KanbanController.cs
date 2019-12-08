using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mimir.API.Commands;
using Mimir.API.Controllers.Abstract;
using Mimir.API.DTO;
using Mimir.API.Queries;
using Mimir.CQRS.Commands;
using Mimir.CQRS.Queries;
using Mimir.Database;
using System.Threading.Tasks;

namespace Mimir.API.Controllers
{
    public class KanbanController : MimirController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public KanbanController(IUserResolver userResolver, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher) 
            : base(userResolver)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> MoveItem([FromBody] KanbanItemMoveRequestDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new MoveKanbanItemCommandHandler.Command(user.ID, dto.BoardId, dto.ItemId, dto.Index, dto.ColumnDestId, dto.Timestamp));

            return await KanbanStateResult(dto.BoardId);
        }

        [HttpPost]
        public async Task<IActionResult> MoveColumn([FromBody] KanbanColumnMoveRequestDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new MoveKanbanColumnCommandHandler.Command(user.ID, dto.BoardId, dto.ColumnId, dto.Index, dto.Timestamp));

            return await KanbanStateResult(dto.BoardId);
        }

        [HttpPut]
        public async Task<IActionResult> Column([FromBody] KanbanColumnAddRequestDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new AddKanbanColumnCommandHandler.Command(user.ID, dto.BoardId, dto.Name));

            return await KanbanStateResult(dto.BoardId);
        }

        [HttpPut]
        public async Task<IActionResult> Item([FromBody] KanbanItemAddRequestDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new AddKanbanItemCommandHandler.Command(user.ID, dto.BoardId, dto.Name, dto.ColumnId));

            return await KanbanStateResult(dto.BoardId);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Item(int id)
        {
            var result = await _queryDispatcher.DispatchAsync(new GetItemDetailsQueryHandler.Query(GetUser().ID, id));
            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Item([FromBody] KanbanItemEditRequestDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new EditKanbanItemCommandHandler.Command(user.ID, dto.BoardId, dto.ItemId, dto.Name, dto.Description, dto.AssigneeId));

            return await KanbanStateResult(dto.BoardId);
        }

        [HttpDelete]
        public async Task<IActionResult> Column([FromBody] KanbanColumnRemoveRequestDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new RemoveKanbanColumnCommandHandler.Command(user.ID, dto.BoardId, dto.ColumnId));

            return await KanbanStateResult(dto.BoardId);
        }

        [HttpPatch]
        public async Task<IActionResult> Column([FromBody] KanbanColumnEditRequestDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new EditKanbanColumnCommandHandler.Command(user.ID, dto.BoardId, dto.ColumnId, dto.Name));

            return await KanbanStateResult(dto.BoardId);
        }

        

        [HttpDelete]
        public async Task<IActionResult> Item([FromBody] KanbanItemRemoveRequestDTO dto)
        {
            var user = GetUser();
            await _commandDispatcher.DispatchAsync(
                new RemoveKanbanItemCommandHandler.Command(user.ID, dto.BoardId, dto.ItemId));

            return await KanbanStateResult(dto.BoardId);
        }

        [HttpGet("{boardId}")]
        public Task<IActionResult> Board(int boardId)
        {
            return KanbanStateResult(boardId);
        }

        private async Task<IActionResult> KanbanStateResult(int boardId)
            => Ok(await _queryDispatcher.DispatchAsync(new KanbanStateQueryHandler.Query(GetUser().ID, boardId)));
    }
}
