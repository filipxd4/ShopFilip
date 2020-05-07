using ShopFilip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Interfaces
{
    public interface IOrderLogic
    {
        Task<IQueryable<Order>> GetUserOrders(string idOfUser);
    }
}
