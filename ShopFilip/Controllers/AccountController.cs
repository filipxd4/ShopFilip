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
using Microsoft.AspNetCore.Authorization;
using ShopFilip.Interfaces;

namespace ShopFilip.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signManager;
        private EfDbContext _context;
        private IOrderLogic _orderLogic;

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public AccountController(EfDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signManager, IOrderLogic orderLogic)
        {
            _context = context;
            _userManager = userManager;
            _signManager = signManager;
            _orderLogic = orderLogic;
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
            ModelState.AddModelError("", "Żle wpisane dane.");
            return View();
        }

        public IActionResult MainPage()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("MainPage", "Home");
        }

        public IActionResult ErrorPage()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Orders()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserOrders()
        {
            var usaer = await GetCurrentUserAsync();
            var userId = usaer?.Id;
            var order = await _orderLogic.GetUserOrders(userId);
            return View(order);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            var model = new Login { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        [Authorize]
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

        public async Task<IActionResult> Index(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.Email = user.Email;
            return View();
        }


        [Authorize]
        public async Task<IActionResult> ManageAccount()
        {
            var usaer = await GetCurrentUserAsync();
            var useraId = usaer?.Id;
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                ApplicationUser user;
                if (User.IsInRole("Admin"))
                {
                    user = await _userManager.FindByIdAsync(useraId);
                    ViewBag.Role = "Admin";
                }
                else
                {
                    user = await _userManager.FindByIdAsync(useraId);
                    ViewBag.Role = "User";
                }
                return View(user);
            }
            else
            {
                return this.RedirectToAction("ErrorPage", "Account");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string userId)
        {
            if (userId == null)
                return ErrorPage();

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return ErrorPage();
            return View(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(string userId, Register model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user.Email = model.Email;
            user.Street = model.Street;
            user.PostalCode = model.PostalCode;
            user.Town = model.Town;
            user.PhoneNumber = model.PhoneNumber;
            user.Street = model.Street;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index", new { userId });
        }
    }
}