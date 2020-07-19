using System;
using System.Collections.Generic;
using System.Text;

namespace Stazh.Core.Data.Models
{
    public class StorageConfig
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string FileContainer { get; set; }
        public string ThumbnailContainer { get; set; }
        public string Path { get; set; }
        public string ConnectionString { get; set; }
    }
}
