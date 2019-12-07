using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mimir.API.Controllers.Abstract;
using Mimir.Database;

namespace Mimir.API.Controllers
{
    public class CommentController : MimirController
    {
        public CommentController(IUserResolver userResolver) : base(userResolver)
        {
        }

        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        public async Task<IActionResult> Add()
        {
            return Ok();
        }

        public async Task<IActionResult> Delete()
        {
            return Ok();
        }

        public async Task<IActionResult> Edit()
        {
            return Ok();
        }
    }
}
