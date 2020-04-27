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
using System.Threading.Tasks;

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

        public async Task<List<OrderProduct>> GetUserOrders(string idOfUser)
        {
            var aaa = await _userManager.FindByIdAsync(idOfUser);
            var orders = _context.Orders.Where(x => x.ApplicationUser.Id == idOfUser).Include(c => c.Products);
            List<OrderProduct> orderProd = new List<OrderProduct>();
            foreach (var item in orders)
            {
                string status = await _payULogic.GetStatusOfOrderAsync(idOfUser);
                
                if (item.StatusOrder == "New" && status == "SUCCESS")
                {
                    item.StatusOrder = "Paid";
                    await _userManager.UpdateAsync(aaa);
                }
                List<PoductQuantityAtribute> prQ = new List<PoductQuantityAtribute>();
                List<Product> productList = new List<Product>();
                foreach (var itemo in item.Products)
                {
                    try
                    {
                        Product product = _context.ProductsData.Where(x => x.Id == itemo.IdOfProduct).Include(x => x.Photos).First();
                        if (!productList.Contains(product))
                        {
                            productList.Add(product);
                        }
                        prQ.Add(new PoductQuantityAtribute(productList, itemo.Quantity, itemo.Value));
                    }
                    catch
                    {
                        //throw new InvalidOperationException("Brak produktu");
                    }
                }
                DateTime dateTime = DateTime.Parse(item.DateOfOrder);
                orderProd.Add(new OrderProduct(prQ, dateTime, item.OrderId, item.StatusOrder));
            }
            var sortedOrderList = orderProd.OrderByDescending(x => x.OrderDate);
            return orderProd;
        }
    }
}
