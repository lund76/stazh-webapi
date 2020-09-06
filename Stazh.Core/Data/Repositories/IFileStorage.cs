using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Stazh.Core.Data.Entities;
using Stazh.Core.Data.Models;
using Stazh.Core.Data.Models.Api;

namespace Stazh.Core.Data.Repositories
{
    public interface IFileStorage
    {
        bool UploadToFileStorage(Stream fileStream, string fileName);
        bool DeleteStorage();
        string GetFileUrl(string fullFilePath);

    }
}
