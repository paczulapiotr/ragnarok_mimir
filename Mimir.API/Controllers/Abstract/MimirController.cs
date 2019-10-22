using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mimir.API.Controllers.Filters;

namespace Mimir.API.Controllers.Abstract
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [ExceptionHandlerFilter]
    public class MimirController : Controller
    {
    }
}
