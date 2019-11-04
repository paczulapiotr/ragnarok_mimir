using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Mimir.API.Controllers.Filters;
using Mimir.API.Result;
using Mimir.Core.Models;
using Mimir.Database;

namespace Mimir.API.Controllers.Abstract
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ExceptionHandlerFilter]
    public class MimirController : Controller
    {
        protected readonly IUserResolver _userResolver;

        public MimirController(IUserResolver userResolver)
        {
            _userResolver = userResolver;
        }

        protected AppUser GetUser() => _userResolver.GetUser(User);

        public override OkObjectResult Ok([ActionResultObjectValue] object value)
        {
            return base.Ok(new ApiJsonResponse(value));
        }
    }
}
