using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopFilip.IdentityModels;
using ShopFilip.Interfaces;
using ShopFilip.Models;
using System.Threading.Tasks;

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
            var user = await GetCurrentUserAsync();
            return View(await _orderLogic.GetUserOrders(user?.Id));
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            return View(new Login { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("MainPage", nameof(AccountController));
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
                if (User.IsInRole("Admin"))
                    ViewBag.Role = "Admin";
                else
                    ViewBag.Role = "User";
                
                var user = await _userManager.FindByIdAsync(useraId);
                return View(user);
            }
            else
            {
                return this.RedirectToAction("ErrorPage", "Account");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var usaer = await GetCurrentUserAsync();
            var useraId = usaer?.Id;
            if (useraId == null)
                return ErrorPage();

            var user = await _context.Users.FindAsync(useraId);
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