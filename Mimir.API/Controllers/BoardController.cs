using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mimir.API.Controllers.Abstract;
using Mimir.API.Queries;
using Mimir.CQRS.Queries;
using Mimir.Database;

namespace Mimir.API.Controllers
{
    public class DTOO
    {
        public string Name { get; set; }
        public int[] IgnoreUserIds { get; set; }
        public int? BoardId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
    public class BoardController : MimirController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public BoardController(IUserResolver userResolver, IQueryDispatcher queryDispatcher) : base(userResolver)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Participants([FromQuery]DTOO dto)
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
