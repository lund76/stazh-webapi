using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Repositories;


namespace Stazh.Data.EFCore.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository

    {
        public ItemRepository(StazhDataContext context) : base(context)
        {

        }

        public IEnumerable<Item> GetBaseItemsForUser(string externalUserId)
        {
            var items = StazhDataContext.Items.Include(u => u.Owner).Include(f => f.ItemAttachments).Where(ext =>
                ext.Owner.ExternalUniqueId == externalUserId && (ext.ParentItemId == 0 || ext.ParentItemId == null));

            return items;
        }

        public IEnumerable<Item> GetChildItemsForUserFrom(string externalUserId, int itemId)
        {
            var item = StazhDataContext.Items.Include(o => o.Owner)
                .Include(c => c.Children)
                .Include(f => f.ItemAttachments)
                .Where(i => externalUserId == i.Owner.ExternalUniqueId && i.Id == itemId);

            return item.SingleOrDefault()?.Children; 
        }

     

        public StazhDataContext StazhDataContext => Context as StazhDataContext;


    }
}
