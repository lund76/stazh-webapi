using System;
using System.Collections.Generic;
using System.Text;

namespace Stazh.Core.Data.Entities
{
    public class Item
    {
        public int Id { get; set;  }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}
