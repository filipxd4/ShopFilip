using ShopFilip.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class OrderedProduct
    {
        public int Id { get; set; }

        public Product Product { get; set; }
        
        public SizeOfPruduct Size { get; set; }

        public int Quantity { get; set; }

        public OrderedProduct(Product product, SizeOfPruduct size, int quantity)
        {
            this.Product = product;
            this.Size = size;
            this.Quantity = quantity;
        }

        public OrderedProduct()
        {
        }
    }
}
