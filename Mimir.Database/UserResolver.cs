using System.Linq;
using System.Security.Claims;
using Mimir.Core.CommonExceptions;
using Mimir.Core.Models;

namespace Mimir.Database
{

    public class UserResolver : IUserResolver
    {
        private readonly MimirDbContext _dbContext;

        public UserResolver(MimirDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AppUser GetUser(ClaimsPrincipal claimsPrincipal)
        {
            var authId = claimsPrincipal.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (string.IsNullOrWhiteSpace(authId))
            {
                throw new UnauthorizedException("Given user is not valid");
            }

            return _dbContext.AppUsers.FirstOrDefault(x => x.AuthID == authId);
        }

        public AppUser GetUser(string authId)
        {
            return _dbContext.AppUsers.FirstOrDefault(x => x.AuthID == authId);
        }
    }
}
