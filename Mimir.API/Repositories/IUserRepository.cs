using Mimir.Core.Models;

namespace Mimir.API.Repositories
{
    public interface IUserRepository
    {
        AppUser CreateUser(string authId, string name);
    }
}
