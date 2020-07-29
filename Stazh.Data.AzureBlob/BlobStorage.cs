using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Models;
using Stazh.Core.Data.Repositories;

using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Stazh.Data.AzureBlob.Helpers;

namespace Stazh.Data.AzureBlob
{
    public class BlobStorage : IFileStorage
    {
        public bool UploadToFileStorage(Stream fileStream, StorageConfig storageConfig, string fileName)
        {
            var blobClient = StorageHelper.GetBlobClientFrom(storageConfig,fileName,true);
            var res = blobClient.Upload(fileStream);
            return (res.GetRawResponse().Status > 199 && res.GetRawResponse().Status < 300);
        }

        public bool DeleteContainer(StorageConfig storageConfig)
        {
            var res = StorageHelper.DeleteContainer(storageConfig);
            return (res < 300 && res > 199);
        }
    }
}
