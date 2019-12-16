using Mimir.Core.Models;

namespace Mimir.API.Services
{
    public interface IContextUserResolver
    {
        AppUser GetCurrentUser();
        AppUser GetUser(string authId);
    }
}
