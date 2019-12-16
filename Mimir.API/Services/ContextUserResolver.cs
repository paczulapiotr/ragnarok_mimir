using System;
using Microsoft.AspNetCore.Http;
using Mimir.Core.Models;
using Mimir.Database;

namespace Mimir.API.Services
{
    public class ContextUserResolver : IContextUserResolver
    {
        private readonly IUserResolver _userResolver;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextUserResolver(
            IUserResolver userResolver, 
            IHttpContextAccessor httpContextAccessor)
        {
            _userResolver = userResolver;
            _httpContextAccessor = httpContextAccessor;
        }
        public AppUser GetCurrentUser()
        {
            return _userResolver.GetUser(_httpContextAccessor.HttpContext.User);
        }

        public AppUser GetUser(string authId)
        {
            return _userResolver.GetUser(authId);
        }
    }
}
