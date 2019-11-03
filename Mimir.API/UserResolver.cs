using System.Linq;
using System.Security.Claims;
using Mimir.Core.Models;
using Mimir.Database;

namespace Mimir.API
{
    public interface IUserResolver
    {
        AppUser GetUser(ClaimsPrincipal claimsPrincipal);
    }

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
                return null;
            }

            return _dbContext.AppUsers.FirstOrDefault(x => x.AuthID == authId);
        }
    }
}
