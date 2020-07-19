using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Models;

namespace Stazh.Core.Data.Repositories
{
    public interface IFileStorage
    {
        bool UploadToFileStorage(Stream fileStream, StorageConfig storageConfig, string fileName);
        bool DeleteContainer(StorageConfig storageConfig);
    }
}
