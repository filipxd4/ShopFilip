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
using static ShopFilip.Helpers.PayUModel;
using Newtonsoft.Json;
using System.Net;
using ShopFilip.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private EfDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public string accessToken { get; set; }
        public string Uri { get; set; }
        public string OrderId = "";

        public CartController(EfDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var cart = SesionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
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
            if (SesionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                var productModel = _context.ProductsData.Where(x=>x.Id==id).Include(x=>x.Photos).First();
                List<Item> cart = new List<Item>();
                cart.Add(new Item { Product = productModel, Quantity = number, Size=size});
                SesionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SesionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = isExist(id,size);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    var productModel = _context.ProductsData.Where(x => x.Id == id).Include(x => x.Photos).First();
                    cart.Add(new Item { Product = productModel, Quantity = number, Size = size });
                }
                SesionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<Item> cart = SesionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SesionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<Item> cart = SesionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        private int isExist(int id, string size)
        {
            List<Item> cart = SesionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id) && cart[i].Size.Equals(size))
                {
                    return i;
                }
            }
            return -1;
        }

        [HttpGet]
        [Authorize]
        private async Task<IActionResult> MyOrders(string id)
        {
            var aaa = await _userManager.FindByIdAsync(id);
            var b = aaa.OrdersList;
            return View();
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> myAction(int Price, string Ip,string returnurl="")
        {
            var usaer = await GetCurrentUserAsync();
            var useraId = usaer?.Id;
            if (returnurl=="")
            {
                List<Item> cart = SesionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                if (useraId == null)
                {
                    return NotFound();
                }
                var userProp = await _context.Users.FindAsync(useraId);
                await GetAccessTokenAsync(userProp, Price, cart, Ip);
                return Redirect(Uri);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        public async Task GetAccessTokenAsync(ApplicationUser user, int Price, List<Item> itemFromCart,string ip)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://secure.payu.com/pl/standard/user/oauth/authorize"))
                {
                    request.Headers.TryAddWithoutValidation("Host", "secure.payu.com");
                    request.Content = new StringContent("grant_type=client_credentials&client_id=145227&client_secret=12f071174cb7eb79d4aac5bc2f07563f", Encoding.UTF8, "application/x-www-form-urlencoded");
                    try
                    {
                        var response = await httpClient.SendAsync(request);
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var objResponse1 = JsonConvert.DeserializeObject<RootObject2>(jsonString);
                        accessToken = objResponse1.access_token;
                        await Order(user, Price, itemFromCart,ip);
                        Response.Redirect(Uri);
                    }
                    catch (Exception)
                    {
                        ErrorPage();
                    }

                }
            }
        }

        private IActionResult ErrorPage()
        {
            return View();
        }

        [Route("sucess")]
        public IActionResult Sucess()
        {
            return View();
        }

        public async Task Order(ApplicationUser user, int price, List<Item> itemfromCart, string ip)
        {
            string ProperPrice = (price*100).ToString();
            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };


            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://secure.payu.com/api/v2_1/orders"))
                {
                    request.Headers.TryAddWithoutValidation("Cache-Control", "no-cache");
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken + "");
                    var stringToPayU = "{\n\"notifyUrl\": \"https://your.eshop.com/notify\"," +
                        "\n\"continueUrl\":\"https://localhost:44380/cart/sucess\"," +
                        "\n\"customerIp\":\""+ip+"\"," +
                        "\n\"merchantPosId\": \"145227\"," +
                        "\n\"description\": \"Filip Shop\"," +
                        "\n\"currencyCode\": \"PLN\",\n\"totalAmount\": \"" + ProperPrice + "\",\n\"buyer\": {\n       " +
                        "\"email\": \"" + user.Email + "\",\n\"phone\": \"988909909\",\n\"firstName\": \"" + user.Name + "\",\n  " +
                        "\"lastName\": \"" + user.Surname + "\",\n\"language\": \"pl\"," +
                        "\n\"delivery\": {\n" +
                        "\"street\": \"" + user.Street + "\",\n\"postalCode\": \"" + user.PostalCode + "\",\n\"city\": \"" + user.Town + "\"\n}" +
                        "\n}," +
                        "\n\"products\": [\n";
                    if (itemfromCart.Count() == 1)
                    {
                        stringToPayU += "{\n\"name\": \"" + itemfromCart.First().Product.Name + "\",\n\"unitPrice\": \"" + itemfromCart.First().Product.Price + "\",\n \"quantity\": \"" + itemfromCart.First().Quantity + "\"\n}\n";
                    }
                    else
                    {
                        for (int i = 0; i < itemfromCart.Count(); i++)
                        {
                            if (i == itemfromCart.Count() - 1)
                            {
                                stringToPayU += "{\n\"name\": \"" + itemfromCart[i].Product.Name + " "+ itemfromCart[i].Size +"\",\n\"unitPrice\": \"" + itemfromCart[i].Product.Price + "\",\n \"quantity\": \"" + itemfromCart[i].Quantity + "\"\n}\n";
                            }
                            else
                            {
                                stringToPayU += "{\n\"name\": \"" + itemfromCart[i].Product.Name + " " + itemfromCart[i].Size + "\",\n\"unitPrice\": \"" + itemfromCart[i].Product.Price + "\",\n \"quantity\": \"" + itemfromCart[i].Quantity + "\"\n},\n";
                            }
                        }
                    }
                    stringToPayU += "]\n}";

                    request.Content = new StringContent(stringToPayU, Encoding.UTF8, "application/json");
                    var response = await httpClient.SendAsync(request);
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var objResponse1 = JsonConvert.DeserializeObject<RootObject>(jsonString);
                    Uri = objResponse1.redirectUri;
                    OrderId = objResponse1.orderId;
                    await SaveOrderToDatabase(OrderId, itemfromCart, user, price.ToString());
                }
            }
        }

        private async Task SaveOrderToDatabase(string orderId, List<Item> listOfProducts, ApplicationUser user,string price)
        {
            var aaa = await _userManager.FindByIdAsync(user.Id);
            Order order = new Order();
            order.DateOfOrder = DateTime.Now.ToShortDateString();
            order.OrderId = orderId;
            List<ProductsId> productId = new List<ProductsId>();
            foreach (var item in listOfProducts)
            {
                var a = _context.ProductsData.Where(x => x.Id == item.Product.Id).Include(c => c.ProductAtribute).First();

                ProductsId product = new ProductsId();
                product.IdOfOrder = orderId;
                product.Quantity = item.Quantity;
                product.IdOfProduct = item.Product.Id;
                product.Value = item.Size;
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
            order.Products = productId;
            order.StatusOrder = "New";
            order.UserId = user.Id;
            order.Price = price;
            List<Order> orderList = new List<Order>();
            orderList.Add(order);
            if (aaa.OrdersList == null)
            {
                aaa.OrdersList = new List<Order>();
            }

            aaa.OrdersList.Add(order);
            await _userManager.UpdateAsync(aaa);
        }
    }
}
