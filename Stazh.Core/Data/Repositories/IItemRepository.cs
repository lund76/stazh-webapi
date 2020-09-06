using System.Collections.Generic;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Models.Api;

namespace Stazh.Core.Data.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        IEnumerable<Item> GetBaseItemsForUser(string externalUserId);
        IEnumerable<Item> GetChildItemsForUserFrom(string externalUserId , int itemId);
    }

}