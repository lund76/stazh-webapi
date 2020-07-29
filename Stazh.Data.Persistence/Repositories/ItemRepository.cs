using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Repositories;


namespace Stazh.Data.EFCore.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository

    {
        public ItemRepository(StazhDataContext context) : base(context)
        {

        }

        public StazhDataContext StazhDataContext => Context as StazhDataContext;
    }
}
