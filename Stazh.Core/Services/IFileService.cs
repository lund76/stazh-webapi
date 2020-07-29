using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Stazh.Core.Data.Models;
using Stazh.Core.Data.Repositories;
using Stazh.Core.Data.Models.Api;

namespace Stazh.Core.Services
{
    public interface IFileService
    {
        bool SaveFile(Stream dataFiles,StorageConfig storageConfig, string fileName);
        Task<bool> SaveFileAsync(Stream openReadStream, StorageConfig storageConfig, string fileFileName);
    }
}
