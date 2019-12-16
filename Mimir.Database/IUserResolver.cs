using System.Security.Claims;
using Mimir.Core.Models;

namespace Mimir.Database
{
    public interface IUserResolver
    {
        AppUser GetUser(ClaimsPrincipal claimsPrincipal);
        AppUser GetUser(string authId);
    }
}
