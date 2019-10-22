using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mimir.API.Result;
using Mimir.Core.CommonExceptions;
using System;

namespace Mimir.API.Controllers.Filters
{
    public class ExceptionHandlerFilterAttribute : ExceptionFilterAttribute
    {
        public ExceptionHandlerFilterAttribute()
        {
        }

        private IActionResult _Forbidden() => new ForbidResult();

        private IActionResult _BadRequest(Exception ex) => new BadRequestObjectResult(new ApiJsonResponse(ApiMessage.Error(ex.Message)));

        private IActionResult _BadRequest(ArgumentNullException ex) => new BadRequestObjectResult(new ApiJsonResponse(ApiMessage.Error(ex.Message)));

        private IActionResult _BadRequest(ArgumentException ex) => new BadRequestObjectResult(new ApiJsonResponse(ApiMessage.Error(ex.Message)));

        private IActionResult _Conflict(ConflictException ex) => new ConflictObjectResult(new ApiJsonResponse(ApiMessage.Error(ex.Message)));

        private IActionResult _NotFound(NotFoundException ex) => new NotFoundObjectResult(new ApiJsonResponse(ApiMessage.Error(ex.Message)));

        private IActionResult _Unauthorized(UnauthorizedException ex) => new UnauthorizedObjectResult(new ApiJsonResponse(ApiMessage.Error(ex.Message)));

        protected IActionResult HandleException(Exception ex)
        {
            switch (ex)
            {
                case ConflictException e: return _Conflict(e);
                case NotFoundException e: return _NotFound(e);
                case ForbiddenException _: return _Forbidden();
                case UnauthorizedException e: return _Unauthorized(e);
                case ArgumentNullException e: return _BadRequest(e);
                case ArgumentException e: return _BadRequest(e);
                default:
                    return _BadRequest(ex);
            }
        }


        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            var ex = context.Exception;
            context.Result = HandleException(ex);
            context.ExceptionHandled = true;
        }
    }
}
