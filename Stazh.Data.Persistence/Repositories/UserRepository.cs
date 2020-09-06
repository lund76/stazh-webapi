using System.Linq;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Repositories;

namespace Stazh.Data.EFCore.Repositories
{
    public class UserRepository :Repository<User>,IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {

        }

        public User GetUserByExternalUniqueId(string uniqueStringId)
        {
            return SingleOrDefault(usr => usr.ExternalUniqueId == uniqueStringId);
        }

        public StazhDataContext StazhDataContext => Context as StazhDataContext;
    }
}