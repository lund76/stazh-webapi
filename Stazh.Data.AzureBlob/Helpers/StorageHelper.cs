using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Stazh.Core.Data.Models;

namespace Stazh.Data.AzureBlob.Helpers
{
    public class StorageHelper
    {
        public static BlobClient GetBlobClientFrom(StorageConfig storageConfig,string fileName, bool createNewContainer)
        {
             var blobClient = CreateOrResolveBlob(storageConfig,fileName,createNewContainer);
             return blobClient;
        }

        public static BlobServiceClient GetBlobServiceClientFrom(StorageConfig storageConfig)
        {
            var blobServiceClient = new BlobServiceClient(storageConfig.ConnectionString);
            return blobServiceClient;
        }

        public static int  DeleteContainer(StorageConfig storageConfig)
        {
            BlobContainerClient blobContainerClient  = new BlobContainerClient(storageConfig.ConnectionString,storageConfig.FileContainer);
            return blobContainerClient.DeleteIfExists().GetRawResponse().Status;

        }


        private static BlobClient CreateOrResolveBlob(StorageConfig storageConfig,string fileName, bool createNewContainer = true)
        {
            
            BlobContainerClient blobContainerClient;
            BlobClient blobClient = null;
            if (!string.IsNullOrEmpty(storageConfig.ConnectionString))
            {
                blobContainerClient = new BlobContainerClient(storageConfig.ConnectionString,storageConfig.FileContainer);
                blobClient = new BlobClient(storageConfig.ConnectionString,storageConfig.FileContainer,fileName);
            }
            else if (!string.IsNullOrEmpty(storageConfig.Path))
            {
                var storageCredential = new StorageSharedKeyCredential(storageConfig.AccountName,storageConfig.AccountKey);
                var fullPath = String.Format(storageConfig.Path, storageConfig.AccountName, storageConfig.FileContainer,
                    fileName);
                
                blobContainerClient = new BlobContainerClient(new Uri(fullPath),storageCredential);
                blobClient = new BlobClient(new Uri(fullPath + "/" + fileName),storageCredential);
            }
            else
            {
                throw new MissingFieldException("Path or Connection string must have a value");
            }

            if (createNewContainer)
                blobContainerClient.CreateIfNotExists();
            else if(!blobContainerClient.Exists())
                throw new Exception("Container doesn't exist");
            return blobClient;
        }
    }

   
}
