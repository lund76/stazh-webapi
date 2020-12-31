using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Stazh.Core.Data;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Models.Api;
using Stazh.Core.Helpers;
using Stazh.Core.Services;

namespace Stazh.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public ItemService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }
        public ApiItem InsertNewItem(Item itemToInsert)
        { 
            _unitOfWork.Items.Add(itemToInsert);
            var completed = _unitOfWork.Complete();
            var itemCreated = new ApiItem(itemToInsert,
                _fileService.GetThumbnailUrls(itemToInsert, itemToInsert.Owner.ExternalUniqueId));
                //{Id = itemToInsert.Id, Name = itemToInsert.Name, Success = completed == 1};
            return itemCreated;
        }

        public Item FindParentFromName(string itemName)
        {
            var item = _unitOfWork.Items.Find(i => i.Name.ToLower() == itemName.ToLower()).FirstOrDefault();
            return item;
        }

        public Item FindParentFromId(string itemId)
        {
            var item = _unitOfWork.Items.Find(i => i.Id.ToString() == itemId).FirstOrDefault();
            return item;
        }

        public Task<SavedFileModel> AddFile(Stream stream, string fileName, string userIdentity)
        {
            var originalFileName = fileName;
            var newFileName = FileHelper.ObscureFileName(fileName);
            var userAndFileName = $"{userIdentity}\\{newFileName}";

            var saveFileModel = new SavedFileModel {OriginalFileName = originalFileName, UniqueFilename = newFileName};
           
            try
            {
                var success = _fileService.SaveFileAsync(stream,userAndFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return Task.FromResult(saveFileModel);

        }

        public IEnumerable<ApiItem> FindAllBaseItems(string externalUserId)
        {
            var items = _unitOfWork.Items.GetBaseItemsForUser(externalUserId).Select(apiItm => new ApiItem(apiItm,_fileService.GetThumbnailUrls(apiItm, externalUserId)));
            
            return items;
        }

        public  IEnumerable<ApiItem> GetChildItemsFor(string externalUserId ,int id)
        {
            var items = _unitOfWork.Items.GetChildItemsForUserFrom(externalUserId, id);
         
            var apiItems =  items?.Select( i => new ApiItem(i, _fileService.GetThumbnailUrls(i, externalUserId)));
            
            return apiItems;
        }

        public void DeleteItem(string userId, int id)
        {
            var itm = _unitOfWork.Items.Get(id);
            if(userId == itm.Owner.ExternalUniqueId)
                _unitOfWork.Items.Remove(itm);
            _unitOfWork.Complete();
        }

        public Item GetItem(int itemId)
        {
            var itm = _unitOfWork.Items.Get(itemId);
            return itm;
        }

        public void UpdateItem(Item item)
        {
            var itm =_unitOfWork.Items.Get(item.Id);
            itm = item;
            _unitOfWork.Complete();
        }
    }
}
