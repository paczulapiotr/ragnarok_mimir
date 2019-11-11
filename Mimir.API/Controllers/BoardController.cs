using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mimir.API.Commands;
using Mimir.API.Controllers.Abstract;
using Mimir.API.DTO;
using Mimir.API.Queries;
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

        [HttpPut]
        public async Task<IActionResult> Create([FromBody]CreateBoardDTO dto)
        {
            await _commandDispatcher.DispatchAsync(new CreateBoardCommandHandler.Command
            {
                UserId = GetUser().ID,
                Name = dto.Name,
                ParticipantIds = dto.ParticipantIds
            });
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Participants([FromQuery]GetBoardParticipantsDTO dto)
        {
            var result = await _queryDispatcher.DispatchAsync(
                new SearchBoardParticipantsQueryHandler.Query
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
    }
}
