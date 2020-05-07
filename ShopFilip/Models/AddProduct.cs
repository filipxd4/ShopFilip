using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class AddProduct
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public string Group { get; set; }

        public string[] Size { get; set; }

        public string[] Quantity { get; set; }

        public string Description { get; set; }

        public string Gender { get; set; }

        public string Table { get; set; }

        public string[] Photos { get; set; }
    }
}
