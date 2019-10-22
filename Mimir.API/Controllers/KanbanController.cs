using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mimir.API.Controllers.Abstract;
using Mimir.API.DTO;

namespace Mimir.API.Controllers
{
    public class KanbanController : MimirController
    {
        [AllowAnonymous]
        public IActionResult MoveItem([FromBody] KanbanItemMoveDTO dto)
        {

            return Json(new KanbanStateDTO());
        }
    }
}
