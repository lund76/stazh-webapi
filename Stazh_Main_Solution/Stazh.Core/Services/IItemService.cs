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
        ApiItemCreated InsertNewItem(Item itemToInsert);
        Item FindParentFromName(string itemName);
        Task<SavedFileModel> AddFile(Stream openReadStream, StorageConfig storageConfig, string fileFileName,string userIdentity);
    }

   
}
