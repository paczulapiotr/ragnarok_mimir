using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mimir.Core.Models;
using Mimir.Database;

namespace Mimir.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MimirDbContext _dbContext;

        public UserRepository(MimirDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AppUser CreateUser(string authId, string name)
        {
            var user = new AppUser
            {
                AuthID = authId,
                Name = name,
            };

            _dbContext.AppUsers.Add(user);
            _dbContext.SaveChanges();
            return user;
        }
    }
}
