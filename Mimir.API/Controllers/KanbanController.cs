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
        public async Task<IActionResult> MoveItem([FromBody] KanbanItemMoveDTO dto)
        {
            _kanbanRepository.MoveItem(dto.BoardId, dto.ItemId, dto.Index, dto.ColumnDestId, dto.Timestamp);

            var result = await _queryHandler.DispatchAsync<KanbanBoardDTO>(
                new KanbanStateQueryHandler.Query(dto.BoardId));

            return Json(new KanbanBoardDTO());
        }

        [AllowAnonymous]
        public async Task<IActionResult> MoveColumn([FromBody] KanbanColumnMoveDTO dto)
        {
            _kanbanRepository.MoveColumn(dto.BoardId, dto.ColumnId, dto.Index, dto.Timestamp);
            return Json(new KanbanBoardDTO());
        }

        [AllowAnonymous]
        public async Task<IActionResult> AddColumn([FromBody] KanbanColumnAddDTO dto)
        {
            _kanbanRepository.AddColumn(dto.BoardId, dto.Name, dto.Timestamp);
            return Json(new KanbanBoardDTO());
        }

        [AllowAnonymous]
        public async Task<IActionResult> AddItem([FromBody] KanbanItemAddDTO dto)
        {
            _kanbanRepository.AddItem(dto.BoardId, dto.Name, dto.ColumnId, dto.Timestamp);
            return Json(new KanbanBoardDTO());
        }
    }
}
