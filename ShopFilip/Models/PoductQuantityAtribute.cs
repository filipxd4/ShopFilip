using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class PoductQuantityAtribute
    {
        public int Quantity { get; set; }

        public string Atribute { get; set; }

        public List<Product> Product{ get; set; }

        public PoductQuantityAtribute(List<Product> product,int quantity, string atribute)
        {
            this.Quantity = quantity;
            this.Product = product;
            this.Atribute = atribute;
        }
    }
}
