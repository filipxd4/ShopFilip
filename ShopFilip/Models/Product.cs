using ShopFilip.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Table { get; set; }

        public Group Group { get; set; }

        public Gender Gender { get; set; }

        public string Description { get; set; }

        public List<Size> Sizes{ get; set; }

        public List<Photos> Photos { get; set; }
    }
}
