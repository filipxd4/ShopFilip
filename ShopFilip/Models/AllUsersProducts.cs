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

        public List<OrderProduct> OrderProducts{ get; set; }

        public DateTime OrderDate { get; set; }

        public AllUsersProducts(List<OrderProduct> orderProducts, ApplicationUser applicationUser, DateTime orderDate)
        {
            this.ApplicationUser = applicationUser;
            this.OrderProducts = orderProducts;
            this.OrderDate = orderDate;
        }
    }
}
