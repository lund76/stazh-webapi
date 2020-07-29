using Stazh.Core.Data;
using Stazh.Core.Data.Repositories;
using Stazh.Data.EFCore.Repositories;

namespace Stazh.Data.EFCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StazhDataContext _context;

        public UnitOfWork(StazhDataContext context)
        {
            _context = context;
            Items = new ItemRepository(_context);
        }

        public void Dispose()
        {
           _context.Dispose();
        }

        public IItemRepository Items { get; }
        public int Complete()
        {
            return _context.SaveChanges();
        }
    }
}
