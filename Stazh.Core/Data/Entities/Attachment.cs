using System;
using System.Collections.Generic;
using System.Text;

namespace Stazh.Core.Data.Entities
{
    public class Attachment : Entity
    {
        public string UniqueAttachmentName { get; set; }
        public string OriginalFileName { get; set; }
        public byte[] Thumbnail { get; set; }
    }
}
