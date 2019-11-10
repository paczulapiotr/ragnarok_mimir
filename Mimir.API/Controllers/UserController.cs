using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Mimir.API.Commands;
using Mimir.API.DTO;
using Mimir.CQRS.Commands;

namespace Mimir.API.Controllers
{
    [EnableCors("IdentityServer")]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public UserController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPut]
        public async Task<IActionResult> Create([FromBody]CreateUserDTO dto)
        {
            await _commandDispatcher.DispatchAsync(
                new CreateUserCommandHandler.Command(dto.AuthId, dto.Name)
            );

            return Ok();
        }
    }
}
