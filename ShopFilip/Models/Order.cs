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

        public Order(Builder builder)
        {
            OrderId = builder.orderId;
            UserId = builder.userId;
            DateOfOrder = builder.dateOfOrder;
            StatusOrder = builder.statusOrder;
            Price = builder.price;
            ApplicationUser = builder.applicationUser;
            Products = builder.products;
        }

        public class Builder
        {
            internal string orderId;
            internal string userId;
            internal string dateOfOrder;
            internal string statusOrder;
            internal string price;
            internal ApplicationUser applicationUser;
            internal List<ProductsId> products;

            public Builder OrderId(string orderId)
            {
                this.orderId = orderId;
                return this;
            }

            public Builder UserID(string userId)
            {
                this.userId = userId;
                return this;
            }

            public Builder DateOfOrder(string dateOfOrder)
            {
                this.dateOfOrder = dateOfOrder;
                return this;
            }

            public Builder StatusOrder(string statusOrder)
            {
                this.statusOrder = statusOrder;
                return this;
            }

            public Builder Price(string price)
            {
                this.price = price;
                return this;
            }

            public Builder ApplicationUser(ApplicationUser applicationUser)
            {
                this.applicationUser = applicationUser;
                return this;
            } 
            
            public Builder Products(List<ProductsId> products)
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
