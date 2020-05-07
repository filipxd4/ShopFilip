using ShopFilip.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class Size
    {
        public int Id { get; set; }

        public SizeOfPruduct SizeOfPruduct { get; set; }

        public int Quantity { get; set; }

        public Size(SizeOfPruduct sizeOfPruduct, int quantity)
        {
            this.SizeOfPruduct = sizeOfPruduct;
            this.Quantity = quantity;
        }
    }   
}
