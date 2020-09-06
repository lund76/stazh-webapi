using Stazh.Core.Data;
using Stazh.Core.Data.Entities;
using Stazh.Core.Services;

namespace Stazh.Services.Implementations
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork; 
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User GetOrCreateUser(string userId)
        {
            var user = _unitOfWork.User.GetUserByExternalUniqueId(userId);
            if (user != null) return user;

            user = new User{ExternalUniqueId = userId};

            _unitOfWork.User.Add(user);
            _unitOfWork.Complete();

            return user;

        }
    }
}