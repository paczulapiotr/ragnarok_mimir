using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mimir.API.Commands.Comment;
using Mimir.API.Controllers.Abstract;
using Mimir.API.DTO.Comment.Request;
using Mimir.API.Queries.Comment;
using Mimir.CQRS.Commands;
using Mimir.CQRS.Queries;
using Mimir.Database;

namespace Mimir.API.Controllers
{
    public class CommentController : MimirController
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public CommentController(IUserResolver userResolver,
            IQueryDispatcher queryDispatcher,
            ICommandDispatcher commandDispatcher) : base(userResolver)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        private async Task<IActionResult> GetComments(int itemId)
        {
            var result = await _queryDispatcher.DispatchAsync(
              new GetQueryHandler.Query
              {
                  ItemId = itemId,
                  UserId = GetUser().ID
              });
            return Ok(result);
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> Get(int itemId) => await GetComments(itemId);

        [HttpPut]
        public async Task<IActionResult> Add(AddCommentRequestDTO dto)
        {
            await _commandDispatcher.DispatchAsync(
                new AddCommandHandler.Command
                {
                    BoardId = dto.BoardId,
                    Content = dto.Content,
                    UserId = GetUser().ID,
                    ItemId = dto.ItemId
                });

            return await GetComments(dto.ItemId);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteCommentRequestDTO dto)
        {
            await _commandDispatcher.DispatchAsync(
            new DeleteCommandHandler.Command
            {
                BoardId = dto.BoardId,
                CommentId = dto.CommentId,
                UserId = GetUser().ID,
                ItemId = dto.ItemId
            });

            return await GetComments(dto.ItemId);
        }

        [HttpPatch]
        public async Task<IActionResult> Edit(EditCommentRequestDTO dto)
        {
            await _commandDispatcher.DispatchAsync(
            new EditCommandHandler.Command
            {
                BoardId = dto.BoardId,
                Content = dto.Content,
                UserId = GetUser().ID,
                ItemId = dto.ItemId,
                CommentId = dto.CommentId
            });

            return await GetComments(dto.ItemId);
        }
    }
}
