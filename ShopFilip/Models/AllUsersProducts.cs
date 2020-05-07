using ShopFilip.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class AllUsersProducts
    {
        public ApplicationUser ApplicationUser { get; set; }

        public List<Order> OrderProducts{ get; set; }

        public AllUsersProducts(List<Order> orderProducts, ApplicationUser applicationUser, DateTime orderDate)
        {
            this.ApplicationUser = applicationUser;
            this.OrderProducts = orderProducts;
        }
    }
}
