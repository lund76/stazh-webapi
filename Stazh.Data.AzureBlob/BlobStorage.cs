using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Models;
using Stazh.Core.Data.Repositories;

using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Stazh.Core.Data.Models.Api;
using Stazh.Data.AzureBlob.Helpers;

namespace Stazh.Data.AzureBlob
{
    public class BlobStorage : IFileStorage
    {
        private readonly StorageConfig _storageConfig;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _container;
        private readonly Uri _accountUri;
        private readonly StorageSharedKeyCredential _storageCredential;
        private readonly  BlobSasBuilder _blobSasBuilder;
        private readonly UriBuilder _sasUri;

        public BlobStorage(StorageConfig storageConfig)
        {
            _storageConfig = storageConfig;
            _accountUri = new Uri("https://" + _storageConfig.AccountName + ".blob.core.windows.net/");

            _blobServiceClient = StorageHelper.GetBlobServiceClientFrom(storageConfig); //new BlobServiceClient(_accountUri);
            _container = _blobServiceClient.GetBlobContainerClient(_storageConfig.ThumbnailContainer);
          

            _storageCredential =  new StorageSharedKeyCredential(_storageConfig.AccountName, _storageConfig.AccountKey);

            _blobSasBuilder = new BlobSasBuilder
            {
                Resource = "c",
                BlobContainerName = _storageConfig.ThumbnailContainer,
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };

            _blobSasBuilder.SetPermissions(BlobContainerSasPermissions.All);

            _sasUri = new UriBuilder(_accountUri)
            {
                Query = _blobSasBuilder.ToSasQueryParameters(_storageCredential).ToString()
            };

        }

        public bool UploadToFileStorage(Stream fileStream, string fileName)
        {
            var blobClient = StorageHelper.GetBlobClientFrom(_storageConfig, fileName, true);
            var res = blobClient.Upload(fileStream);
            return (res.GetRawResponse().Status > 199 && res.GetRawResponse().Status < 300);
        }

        public bool DeleteStorage()
        {
            var res = StorageHelper.DeleteContainer(_storageConfig);
            return (res < 300 && res > 199);
        }

    


        public string GetFileUrl(string fullFilePath)
        {

            string sasBlobUri;
            if (!_container.Exists()) throw  new Exception("Container doesn't exist");
            
            // Create the URI using the SAS query token.
            sasBlobUri = _container.Uri + "/" + fullFilePath + _sasUri.Query;

            return sasBlobUri;
        }
    }
}
