using ShopFilip.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class Order
    {
        public string OrderId { get; set; }

        public string UserId { get; set; }

        public string DateOfOrder { get; set; }

        public string StatusOrder { get; set; }

        public string Price { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public List<ProductsId> Products { get; set; }
    }
}
