using ShopFilip.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class ShoppingCartItem
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public SizeOfPruduct Size { get; set; }
    }
}
