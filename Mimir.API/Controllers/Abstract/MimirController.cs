using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Mimir.API.Controllers.Filters;
using Mimir.API.Result;

namespace Mimir.API.Controllers.Abstract
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ExceptionHandlerFilter]
    public class MimirController : Controller
    {
        public override OkObjectResult Ok([ActionResultObjectValue] object value)
        {
            return base.Ok(new ApiJsonResponse(value));
        }
    }
}
