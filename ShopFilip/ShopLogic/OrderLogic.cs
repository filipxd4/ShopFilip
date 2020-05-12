using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShopFilip.IdentityModels;
using ShopFilip.Interfaces;
using ShopFilip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static ShopFilip.Models.PayUModel;

namespace ShopFilip.ShopLogic
{
    public class OrderLogic : IOrderLogic
    {
        private UserManager<ApplicationUser> _userManager;
        private EfDbContext _context;
        private IPayULogic _payULogic;

        public OrderLogic(UserManager<ApplicationUser> UserManager, EfDbContext Context,IPayULogic PayULogic)
        {
            _userManager = UserManager;
            _context = Context;
            _payULogic = PayULogic;
        }

        public async Task<IQueryable<Order>> GetUserOrders(string idOfUser)
        {
            var orders = _context.Orders.Where(x => x.ApplicationUser.Id == idOfUser).Include(c => c.Products).ThenInclude(d=>d.Product).ThenInclude(e=>e.Photos);
            foreach (var item in orders)
            {
                string status = await _payULogic.GetStatusOfOrderAsync(item.OrderId);
                if (item.Status == Helpers.Status.New && status == "SUCCESS")
                {
                    item.Status = Helpers.Status.Paid;
                    _context.Update(orders);
                }
            }
            var sortedOrderList = orders.OrderByDescending(x => x.DateOfOrder);
            return sortedOrderList;
        }
    }
}
