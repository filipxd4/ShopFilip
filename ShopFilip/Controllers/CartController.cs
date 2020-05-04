﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopFilip.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ShopFilip.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using ShopFilip.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static ShopFilip.Models.PayUModel;
using ShopFilip.Interfaces;

namespace OnlineShop.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private EfDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IPayULogic _payULogic;
        public string OrderId = "";
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public CartController(EfDbContext context, UserManager<ApplicationUser> userManager,IPayULogic PayULogic)
        {
            _context = context;
            _userManager = userManager;
            _payULogic = PayULogic;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var cart = SesionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "cart");
            if (cart!=null)
            {
                ViewBag.cart = cart;
                ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
            }
            return View();
        }

        [Route("buy/{id}")]
        public async Task<IActionResult> Buy(int id, string size, int number)
        {
            if (SesionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "cart") == null)
            {
                var productModel = _context.ProductsData.Where(x=>x.Id==id).Include(x=>x.Photos).First();
                List<ShoppingCartItem> cart = new List<ShoppingCartItem>();
                cart.Add(new ShoppingCartItem { Product = productModel, Quantity = number, Size=size});
                SesionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<ShoppingCartItem> cart = SesionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "cart");
                int index = ifExist(id,size);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    var productModel = _context.ProductsData.Where(x => x.Id == id).Include(x => x.Photos).First();
                    cart.Add(new ShoppingCartItem { Product = productModel, Quantity = number, Size = size });
                }
                SesionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<ShoppingCartItem> cart = SesionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "cart");
            int index = ifExist(id);
            cart.RemoveAt(index);
            SesionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int ifExist(int id,string size=null)
        {
            List<ShoppingCartItem> cart = SesionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (size != null)
                {
                    if (cart[i].Size.Equals(size) && cart[i].Product.Id.Equals(id))
                    {
                        return i;
                    }
                }
                else if(cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MakeOrder(int Price, string Ip,string returnurl="")
        {
            var usaer = await GetCurrentUserAsync();
            var useraId = usaer?.Id;
            if (returnurl=="")
            {
                List<ShoppingCartItem> cartItems = SesionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "cart");
                var user= await _context.Users.FindAsync(useraId);
                var accessToken = await _payULogic.GetAccessTokenAsync();
                var payUResponse = await _payULogic.GeneratePayLink(user, Price, cartItems, Ip, accessToken);
                var jsonPayU = JsonConvert.DeserializeObject<StatusModel>(payUResponse);
                var orderId = jsonPayU.orderId;
                var uri = jsonPayU.redirectUri;
                await SaveOrderToDatabase(orderId, cartItems, user, Price.ToString());
                return Redirect(uri);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Route("sucess")]
        public IActionResult Sucess()
        {
            return View();
        }

        private async Task SaveOrderToDatabase(string orderId, List<ShoppingCartItem> listOfProducts, ApplicationUser user,string price)
        {
            List<OrderedProductsData> productId = new List<OrderedProductsData>();
            foreach (var item in listOfProducts)
            {
                var a = _context.ProductsData.Where(x => x.Id == item.Product.Id).Include(c => c.ProductAtribute).First();

                OrderedProductsData product = new OrderedProductsData();
                product.IdOfOrder = orderId;
                product.Quantity = item.Quantity;
                product.IdOfProduct = item.Product.Id;
                product.Size = item.Size;
                foreach (var itema in a.ProductAtribute)
                {
                    if (itema.Value==item.Size)
                    {
                        itema.Quantity = itema.Quantity - item.Quantity;
                        _context.Update(itema);
                    }
                }
                productId.Add(product);
            }

            Order order = new Order.Builder()
                .DateOfOrder(DateTime.Now.ToShortDateString())
                .OrderId(orderId)
                .Products(productId)
                .StatusOrder("New")
                .UserID(user.Id)
                .Price(price)
                .Build();

            List<Order> orderList = new List<Order>();
            orderList.Add(order);
            if (user.OrdersList == null)
            {
                user.OrdersList = new List<Order>();
            }
            user.OrdersList.Add(order);
            await _userManager.UpdateAsync(user);
        }
    }
}
