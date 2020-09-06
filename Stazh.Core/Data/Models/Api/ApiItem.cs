using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stazh.Core.Data.Entities;
using Stazh.Core.Services;

namespace Stazh.Core.Data.Models.Api
{
    public class ApiItem
    {
        public ApiItem()
        {
            
        }

        public ApiItem(Item itemToConvert, List<string> fileUrls)
        {
            Id = itemToConvert.Id;
            Name = itemToConvert.Name;
            Description = itemToConvert.Description;
            ArchiveDate = itemToConvert.Created;
            if (itemToConvert.Parent != null)
            {
                ParentId = itemToConvert.Parent.Id;
            }

            if (fileUrls != null && itemToConvert.ItemAttachments != null && itemToConvert.ItemAttachments.Any() )
            {
                Pictures = fileUrls;
            }

        }

     

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public IEnumerable<string> Pictures { get; set; }
        public DateTime ArchiveDate { get; set; }
    }
}   
