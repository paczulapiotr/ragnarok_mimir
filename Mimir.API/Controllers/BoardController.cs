using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mimir.API.Commands;
using Mimir.API.Controllers.Abstract;
using Mimir.API.DTO;
using Mimir.API.Queries;
using Mimir.API.Result;
using Mimir.CQRS.Commands;
using Mimir.CQRS.Queries;
using Mimir.Database;

namespace Mimir.API.Controllers
{

    public class BoardController : MimirController
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public BoardController(IUserResolver userResolver, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(userResolver)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet("/api/[controller]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _queryDispatcher.DispatchAsync(
            new GetBoardBaseQueryHandler.Query
            {
                BoardId = id,
                UserId = GetUser().ID
            });

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery]GetBoardsRequestDTO dto)
        {
            var result = await _queryDispatcher.DispatchAsync(
            new GetBoardsQueryHandler.Query
            {
                Owned = dto.Owned,
                Page = dto.Page,
                PageSize = dto.PageSize,
                Search = dto.Search,
                UserId = GetUser().ID
            });

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Create([FromBody]CreateBoardRequestDTO dto)
        {
            var command = new CreateBoardCommandHandler.Command
            {
                UserId = GetUser().ID,
                Name = dto.Name,
                ParticipantIds = dto.ParticipantIds
            };
            await _commandDispatcher.DispatchAsync(command);

            return Json(new ApiJsonResponse(command.BoardId, ApiMessage.Info($"'{dto.Name}' board has been created")));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _commandDispatcher.DispatchAsync(
                new RemoveBoardCommandHandler.Command { BoardID = id, UserId = GetUser().ID });

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _queryDispatcher.DispatchAsync(
                new GetBoardDetailsQueryHandler.Query { BoardId = id, UserId = GetUser().ID });

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit([FromRoute]int id, [FromBody]EditBoardRequestDTO dto)
        {
            await _commandDispatcher.DispatchAsync(
                new EditBoardCommandHandler.Command { UserId = GetUser().ID, BoardId = id, Name = dto.Name });
            
             return Json(new ApiJsonResponse(ApiMessage.Info($"Board name has been changed")));
        }

        [HttpGet]
        public async Task<IActionResult> NewParticipants([FromQuery]SearchNewBoardParticipantsRequestDTO dto)
        {
            var result = await _queryDispatcher.DispatchAsync(
                new SearchNewBoardParticipantsQueryHandler.Query
                {
                    BoardId = dto.BoardId,
                    Name = dto.Name,
                    IgnoreUserIds = dto.IgnoreUserIds,
                    Page = dto.Page,
                    PageSize = dto.PageSize,
                    UserId = GetUser().ID
                });

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Participants([FromQuery]GetBoardParticipantsRequestDTO dto)
        {
            var result = await _queryDispatcher.DispatchAsync(
                new SearchBoardAssigneesQueryHandler.Query
                {
                    BoardId = dto.BoardId,
                    Name = dto.Name,
                    Page = dto.Page,
                    PageSize = dto.PageSize,
                    UserId = GetUser().ID
                });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Participants(int id)
        {
            var result = await _queryDispatcher.DispatchAsync(
                new GetBoardParticipantsQueryHandler.Query { BoardId = id, UserId = GetUser().ID });

            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Participants([FromBody]EditBoardParticipantsRequestDTO dto)
        {
            var userId = GetUser().ID;
            await _commandDispatcher.DispatchAsync(new EditBoardParticipantsCommandHandler.Command
            {
                UserId = userId,
                BoardId = dto.Id,
                ParticipantIds = dto.ParticipantIds
            });

            var result = await _queryDispatcher.DispatchAsync(
                new GetBoardParticipantsQueryHandler.Query { BoardId = dto.Id, UserId = userId });

            return Ok(result);
        }
    }
}
