using System;
using System.Collections.Generic;
using System.Text;

namespace Stazh.Core.Data.Entities
{
    public class Item : Entity
    {
        public int? ParentItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public virtual Item Parent { get;set; }
        public virtual HashSet<Item> Children { get; set; }
        public virtual User Owner { get; set; }
        public virtual HashSet<Attachment> ItemAttachments { get; set; }
        
    }
}
