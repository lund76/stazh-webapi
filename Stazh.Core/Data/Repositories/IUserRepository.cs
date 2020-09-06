using Stazh.Core.Data.Entities;

namespace Stazh.Core.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByExternalUniqueId(string uniqueStringId);
    }
}