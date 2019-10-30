using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mimir.API.Controllers.Abstract;
using Mimir.API.DTO;
using Mimir.API.Queries;
using Mimir.CQRS.Queries;
using Mimir.Kanban;
using System.Threading.Tasks;

namespace Mimir.API.Controllers
{
    public class KanbanController : MimirController
    {
        private readonly IKanbanRepository _kanbanRepository;
        private readonly QueryDispatcher _queryHandler;

        public KanbanController(IKanbanRepository kanbanRepository, QueryDispatcher queryHandler)
        {
            _kanbanRepository = kanbanRepository;
            _queryHandler = queryHandler;
        }
        [AllowAnonymous]
        [HttpPost]
        public Task<IActionResult> MoveItem([FromBody] KanbanItemMoveDTO dto)
        {
            _kanbanRepository.MoveItem(dto.BoardId, dto.ItemId, dto.Index, dto.ColumnDestId, dto.Timestamp);

            return KanbanStateResult(dto.BoardId);
        }

        [AllowAnonymous]
        [HttpPost]
        public Task<IActionResult> MoveColumn([FromBody] KanbanColumnMoveDTO dto)
        {
            _kanbanRepository.MoveColumn(dto.BoardId, dto.ColumnId, dto.Index, dto.Timestamp);

            return KanbanStateResult(dto.BoardId);
        }

        [AllowAnonymous]
        [HttpPost]
        public Task<IActionResult> AddColumn([FromBody] KanbanColumnAddDTO dto)
        {
            _kanbanRepository.AddColumn(dto.BoardId, dto.Name, dto.Timestamp);

            return KanbanStateResult(dto.BoardId);
        }

        [AllowAnonymous]
        [HttpPost]
        public Task<IActionResult> AddItem([FromBody] KanbanItemAddDTO dto)
        {
            _kanbanRepository.AddItem(dto.BoardId, dto.Name, dto.ColumnId, dto.Timestamp);

            return KanbanStateResult(dto.BoardId);
        }

        [AllowAnonymous]
        [HttpPost]
        public Task<IActionResult> RemoveColumn([FromBody] KanbanColumnRemoveDTO dto)
        {
            _kanbanRepository.RemoveColumn(dto.BoardId, dto.ColumnId, dto.Timestamp);

            return KanbanStateResult(dto.BoardId);
        }

        [AllowAnonymous]
        [HttpPost]
        public Task<IActionResult> RemoveItem([FromBody] KanbanItemRemoveDTO dto)
        {
            _kanbanRepository.RemoveItem(dto.BoardId, dto.ItemId, dto.Timestamp);

            return KanbanStateResult(dto.BoardId);
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
