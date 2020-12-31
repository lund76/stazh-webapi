using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Models;
using Stazh.Core.Data.Models.Api;

namespace Stazh.Core.Services
{
    public interface IItemService
    {
        ApiItem InsertNewItem(Item itemToInsert);
        Item FindParentFromName(string itemName);
        Item FindParentFromId(string itemId);
        Task<SavedFileModel> AddFile(Stream openReadStream, string fileName,string userIdentity);
        IEnumerable<ApiItem> FindAllBaseItems(string externalUserId);
        IEnumerable<ApiItem> GetChildItemsFor(string externalUserId, int id);
        void DeleteItem( string userId,int id);
        Item GetItem(int itemId);

        void UpdateItem(Item item);
    }

   
}
