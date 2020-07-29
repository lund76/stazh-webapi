using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Stazh.Core.Data.Models;
using Stazh.Core.Data.Repositories;

namespace Stazh.Data.AzureBlob.Testing
{
    public class AzureBlobIntegrationsTesting
    {
        private ServiceCollection _serviceCollection;
        private ServiceProvider _serviceProvider;
        private StorageConfig _storageConfig;
        private IConfigurationRoot _configuration;
        private IFileStorage _blobStorage;

        [OneTimeSetUp]
        public void MainSetup()
        {
            _storageConfig = new StorageConfig();
            _serviceCollection = new ServiceCollection();
            ConfigureServices(_serviceCollection);
            _serviceProvider = _serviceCollection.BuildServiceProvider();
            _storageConfig.FileContainer = Guid.NewGuid().ToString();
        }

        [OneTimeTearDown]
        public void MainTearDown()
        {
            _storageConfig.ConnectionString = _configuration["AzureConnectionString"];
            bool result = _blobStorage.DeleteContainer(_storageConfig);
        }

        [SetUp]
        public void Setup()
        {
            _storageConfig.AccountKey = _configuration["AzureStorageKey"];
            _storageConfig.AccountName = _configuration["AzureStorageName"];
            _storageConfig.ConnectionString = _configuration["AzureConnectionString"];
            _storageConfig.Path = _configuration["AzureStoragePath"];
            _blobStorage =  _serviceProvider.GetService<IFileStorage>();
        }

        [Test]
        public void UploadFile_WithUriKeyAndName_UploadSuccess()
        {
            var fileContext = GetTestStream();

            _storageConfig.ConnectionString = "";

            var success =_blobStorage.UploadToFileStorage(fileContext.Item1,_storageConfig,fileContext.Item2);
            Assert.That(success,"success");
        }
        [Test]
        public void UploadFile_WithConnectionString_UploadSuccess()
        {
            var fileContext = GetTestStream();
            _storageConfig.Path = "";

            var success = _blobStorage.UploadToFileStorage(fileContext.Item1,_storageConfig,fileContext.Item2);
            Assert.That(success,"success");
            

        }

        [Test]
        public void UploadFile_WithEmptyUriAndConnString_ThrowError()
        {
            var fileContext = GetTestStream();
            _storageConfig.Path = "";
            _storageConfig.ConnectionString = "";

            Assert.Throws(typeof(MissingFieldException), () => _blobStorage.UploadToFileStorage
                (fileContext.Item1, _storageConfig, fileContext.Item2));

            Assert.Catch(() => _blobStorage.UploadToFileStorage
                    (fileContext.Item1, _storageConfig, fileContext.Item2),  "Path or Connection string must have a value"
            );
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddSingleton<IFileStorage, BlobStorage>();
        }

        private static Tuple<Stream, string> GetTestStream()
        {
            var fileName = @"custnr/" + Guid.NewGuid().ToString() + ".txt";
            var fileStream = new MemoryStream(Encoding.UTF8.GetBytes("sample text for file"));
            return new Tuple<Stream, string>(fileStream,fileName);
        }
        
       

       
    }

    
}