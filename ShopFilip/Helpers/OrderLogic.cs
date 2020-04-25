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
using static ShopFilip.Helpers.DataPayU;

namespace ShopFilip.DataBase
{
    public class OrderLogic : IOrderLogic
    {
        private UserManager<ApplicationUser> _userManager;
        private EfDbContext _context;

        public OrderLogic(UserManager<ApplicationUser> UserManager, EfDbContext Context)
        {
            _userManager = UserManager;
            _context = Context;
        }

        public async Task<List<OrderProduct>> GetUserOrders(string idOfUser)
        {
            var aaa = await _userManager.FindByIdAsync(idOfUser);
            var orders = _context.Orders.Where(x => x.ApplicationUser.Id == idOfUser).Include(c => c.Products);
            List<OrderProduct> orderProd = new List<OrderProduct>();
            foreach (var item in orders)
            {
                string status = string.Empty;
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://private-anon-8f04126df6-payu21.apiary-proxy.com/api/v2_1/orders/" + item.OrderId))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer 3e5cac39-7e38-4139-8fd6-30adc06a61bd");
                        var response = await httpClient.SendAsync(request);
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var objResponse1 = JsonConvert.DeserializeObject<RootObject2>(jsonString);
                        status = objResponse1.status.statusCode;
                    }
                }
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
