using System;
using System.Collections.Generic;
using System.Text;
using Stazh.Core.Data.Repositories;

namespace Stazh.Core.Data
{
   public interface IUnitOfWork : IDisposable
    {
        IItemRepository Items { get; }
        IUserRepository User { get; }
    
        int Complete();
    }
}
