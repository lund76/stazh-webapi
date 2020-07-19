using System.IO;
using System.Threading.Tasks;
using Stazh.Core.Data.Models;
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

        public bool SaveFile(Stream fileStream,StorageConfig storageConfig,string fileName)
        {
           return _fileStorage.UploadToFileStorage(fileStream,storageConfig,fileName);
        }

        public Task<bool> SaveFileAsync(Stream fileStream, StorageConfig storageConfig, string fileName)
        {
            var success = _fileStorage.UploadToFileStorage(fileStream, storageConfig, fileName);
            return Task.FromResult(success);
        }
    }

   
}
