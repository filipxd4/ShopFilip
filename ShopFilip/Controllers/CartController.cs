using System;
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
        public IActionResult Buy(int id, string size, int number)
        {
            if (SesionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "cart") == null)
            {
                List<ShoppingCartItem> cart = new List<ShoppingCartItem>();
                var productModel = _context.Products.Where(x=>x.Id==id).Include(x=>x.Photos).First();
                cart.Add(new ShoppingCartItem { Product = productModel, Quantity = number, Size= (SizeOfPruduct)Enum.Parse(typeof(SizeOfPruduct), size)});
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
                    var productModel = _context.Products.Where(x => x.Id == id).Include(x => x.Photos).First();
                    cart.Add(new ShoppingCartItem { Product = productModel, Quantity = number, Size = (SizeOfPruduct)Enum.Parse(typeof(SizeOfPruduct), size) });
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MakeOrder(decimal price, string Ip,string returnurl="")
        {
            if (returnurl=="")
            {
                var user = await GetCurrentUserAsync();
                var userId = user?.Id;
                List<ShoppingCartItem> cartItems = SesionHelper.GetObjectFromJson<List<ShoppingCartItem>>(HttpContext.Session, "cart");
                var userAccount= await _context.Users.FindAsync(userId);
                var accessToken = await _payULogic.GetAccessTokenAsync();
                var payUResponse = await _payULogic.GeneratePayLink(userAccount, price, cartItems, Ip, accessToken);
                var jsonPayU = JsonConvert.DeserializeObject<StatusModel>(payUResponse);
                var orderId = jsonPayU.orderId;
                var uri = jsonPayU.redirectUri;
                await SaveOrderToDatabase(orderId, cartItems, user, price);
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

        private int ifExist(int id, string size = null)
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
                else if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        private async Task SaveOrderToDatabase(string orderId, List<ShoppingCartItem> listOfProducts, ApplicationUser user,decimal price)
        {
            List<OrderedProduct> orderedProducts = new List<OrderedProduct>();
            foreach (var item in listOfProducts)
            {
                var product = _context.Products.Where(x => x.Id == item.Product.Id).Include(c => c.Sizes).First();
                OrderedProduct orderedProduct = new OrderedProduct(product,(SizeOfPruduct)Convert.ToInt32(item.Size), item.Quantity);

                foreach (var itema in product.Sizes)
                {
                    if (itema.SizeOfPruduct==item.Size)
                    {
                        itema.Quantity = itema.Quantity - item.Quantity;
                        _context.Update(itema);
                    }
                }
                orderedProducts.Add(orderedProduct);
            }

            Order order = new Order.Builder()
                .DateOfOrder(DateTime.Now)
                .OrderId(orderId)
                .Products(orderedProducts)
                .Status(ShopFilip.Helpers.Status.New)
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
