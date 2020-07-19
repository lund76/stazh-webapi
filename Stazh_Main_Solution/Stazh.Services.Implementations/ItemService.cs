using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stazh.Core.Data;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Models;
using Stazh.Core.Data.Models.Api;
using Stazh.Core.Data.Repositories;
using Stazh.Core.Helpers;
using Stazh.Core.Services;

namespace Stazh.Services.Implementations
{
    public class ItemService : IItemService
    {
        private IFileService _fileService;
        private IUnitOfWork _unitOfWork;

        public ItemService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }
        public ApiItemCreated InsertNewItem(Item itemToInsert)
        { 
            _unitOfWork.Items.Add(itemToInsert);
            var completed = _unitOfWork.Complete();
            var itemCreated = new ApiItemCreated
                {Id = itemToInsert.Id, Name = itemToInsert.Name, Success = completed == 1};
            return itemCreated;
        }

        public Item FindParentFromName(string itemName)
        {
            var item = _unitOfWork.Items.Find(i => i.Name.ToLower() == itemName.ToLower()).FirstOrDefault();
            return item;
        }

        public Task<SavedFileModel> AddFile(Stream stream, StorageConfig storageConfig, string fileName,string userIdentity)
        {
            var originalFileName = fileName;
            var newFileName = FileHelper.ObscureFileName(fileName);
            var userAndFileName = $"{userIdentity}\\{newFileName}";

            var saveFileModel = new SavedFileModel {OriginalFileName = originalFileName, UniqueFilename = newFileName};
           
            try
            {
                var success = _fileService.SaveFileAsync(stream,storageConfig,userAndFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return Task.FromResult(saveFileModel);

        }
    }
}
