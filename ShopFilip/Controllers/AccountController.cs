using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopFilip.Models;
using ShopFilip.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using static ShopFilip.Helpers.DataPayU;
using Newtonsoft.Json;

namespace ShopFilip.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signManager;
        private EfDbContext _context;

        public AccountController(EfDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager)
        {
            _context = context;
            _userManager = userManager;
            _signManager = signManager;
        }

        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    Town = model.Town,
                    Street = model.Street,
                    PostalCode = model.PostalCode,
                    PhoneNumber = model.PhoneNumber
                };
                var createUser = await _userManager.CreateAsync(user, model.Password);
                if (createUser.Succeeded)
                {
                    await _signManager.SignInAsync(user, false);
                    return RedirectToAction("MainPage", "Home");
                }
            }
            return View();
        }

        public IActionResult MainPage()
        {
            if (User.IsInRole("Admin"))
                return this.RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("MainPage", "Home");
        }

        public IActionResult ErrorPage()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Orders()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders(string id)
        {
            var aaa = await _userManager.FindByIdAsync(id);
            var orders = _context.Orders.Where(x => x.ApplicationUser.Id == id).Include(c => c.Products);

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
                    Product product = _context.ProductsData.Where(x => x.Id == itemo.IdOfProduct).Include(x=>x.Photos).First();
                    if (!productList.Contains(product))
                    {
                        productList.Add(product);
                    }
                    prQ.Add(new PoductQuantityAtribute(productList, itemo.Quantity,itemo.Value));
                }
                DateTime dateTime = DateTime.Parse(item.DateOfOrder);
                orderProd.Add(new OrderProduct(prQ, dateTime, item.OrderId, item.StatusOrder));
            }
            var sortedOrderList = orderProd.OrderByDescending(x => x.OrderDate);
            return View(sortedOrderList);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            var model = new Login { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("MainPage", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(model.Username,
                   model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        if (Url.IsLocalUrl(model.ReturnUrl))
                            return Redirect(model.ReturnUrl);
                        else
                            return RedirectToAction("MainPage", "Account");
                    }
                }
            }
            ModelState.AddModelError("", "Zły login lub hasło");
            return View(model);
        }

        public async Task<IActionResult> Index(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ViewBag.Email = user.Email;
            return View();
        }

        public async Task<IActionResult> ManageAccount(string id)
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                var user = await _userManager.FindByIdAsync(id);
                return View(user);
            }
            else
            {
                return this.RedirectToAction("ErrorPage", "Account");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return ErrorPage();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return ErrorPage();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, Register model)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.Email = model.Email;
            user.Street = model.Street;
            user.PostalCode = model.PostalCode;
            user.Town = model.Town;
            user.PhoneNumber = model.PhoneNumber;
            user.Street = model.Street;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index", new { id });
        }
    }
}