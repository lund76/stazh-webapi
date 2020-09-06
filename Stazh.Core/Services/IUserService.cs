using Stazh.Core.Data.Entities;

namespace Stazh.Core.Services
{
    public interface IUserService
    {
        User GetOrCreateUser(string userId);

    }
}