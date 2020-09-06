using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Models;
using Stazh.Core.Data.Models.Api;
using Stazh.Core.Data.Repositories;
using Stazh.Core.Services;

namespace Stazh.Services.Implementations
{
    public class FileService : IFileService
    {

        private readonly IFileStorage _fileStorage;

        public FileService(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public bool SaveFile(Stream fileStream,string fileName)
        {
           return _fileStorage.UploadToFileStorage(fileStream,fileName);
        }

        public Task<bool> SaveFileAsync(Stream fileStream, string fileName)
        {
            var success = _fileStorage.UploadToFileStorage(fileStream, fileName);
            return Task.FromResult(success);
        }

        public List<string> GetThumbnailUrls(Item item,string uniqueUserID)
        {
            var urls = new List<string>();
            if (item.ItemAttachments == null) return urls;
            foreach (var itm in item.ItemAttachments)
            {
                urls.Add(_fileStorage.GetFileUrl($"{uniqueUserID}/{itm.UniqueAttachmentName}"));
            }

            return urls;
        }
    }
}
