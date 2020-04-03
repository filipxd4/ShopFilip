using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class OrderProduct
    {
        public string OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderStatus { get; set; }

        public List<PoductQuantityAtribute> PoductQuantityAtribute { get; set; }

        public  OrderProduct(List<PoductQuantityAtribute> poductQuantityAtribute, DateTime orderDate, string orderId,string orderStatus)
        {
            this.OrderDate = orderDate;
            this.PoductQuantityAtribute = poductQuantityAtribute;
            this.OrderId = orderId;
            this.OrderStatus = orderStatus;
        }
    }
}
