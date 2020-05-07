using ShopFilip.Helpers;
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

        public DateTime DateOfOrder { get; set; }

        public Status Status { get; set; }

        public decimal Price { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public List<OrderedProduct> Products { get; set; }

        public Order()
        {
        }

        public Order(Builder builder)
        {
            OrderId = builder.orderId;
            DateOfOrder = builder.dateOfOrder;
            Status= builder.status;
            Price = builder.price;
            ApplicationUser = builder.applicationUser;
            Products = builder.products;
        }

        public class Builder
        {
            internal string orderId;
            internal DateTime dateOfOrder;
            internal Status status;
            internal decimal price;
            internal ApplicationUser applicationUser;
            internal List<OrderedProduct> products;

            public Builder OrderId(string orderId)
            {
                this.orderId = orderId;
                return this;
            }

            public Builder DateOfOrder(DateTime dateOfOrder)
            {
                this.dateOfOrder = dateOfOrder;
                return this;
            }

            public Builder Status(Status status)
            {
                this.status = status;
                return this;
            }

            public Builder Price(decimal price)
            {
                this.price = price;
                return this;
            }

            public Builder ApplicationUser(ApplicationUser applicationUser)
            {
                this.applicationUser = applicationUser;
                return this;
            } 
            
            public Builder Products(List<OrderedProduct> products)
            {
                this.products = products;
                return this;
            }

            public Order Build()
            {
                return new Order(this);
            }
        }
    }
}
