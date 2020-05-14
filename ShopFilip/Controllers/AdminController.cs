using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopFilip.Helpers;
using ShopFilip.IdentityModels;
using ShopFilip.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Controllers
{
    [RoleFilter(Role = "Admin")]
    public class AdminController : Controller
    {
        private EfDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironmen;

        public AdminController(EfDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironmen)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironmen = hostingEnvironmen;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllProducts()
        {
            return View(await _context.Products.Include(c => c.Sizes).ToListAsync());
        }

        [HttpGet]
        public IActionResult AddProduct(int? id)
        {
            if (id != null)
            {
                return View(_context.Products.Include(c => c.Sizes).Include(k => k.Photos).Single(x => x.Id == id));
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddNewProduct([FromBody]AddProduct productToAdd)
        {
            if (ModelState.IsValid)
            {
                List<Size> sizes = new List<Size>();
                Product product = new Product();
                var quantityWithoutNull = productToAdd.Quantity.Where(x => x != String.Empty).ToArray();

                for (int i = 0; i < productToAdd.Size.Length; i++)
                {
                    sizes.Add(new Size((SizeOfPruduct)Convert.ToInt32(productToAdd.Size[i]), Convert.ToInt32(quantityWithoutNull[i])));
                }

                product.Name = productToAdd.Name;
                product.Price = Convert.ToInt32(productToAdd.Price);
                product.Description = productToAdd.Description;
                product.Gender = (Gender)Convert.ToInt32(productToAdd.Gender);
                product.Group = (Group)Convert.ToInt32(productToAdd.Group);
                product.Sizes = sizes;
                List<Photos> photos= new List<Photos>();

                foreach (var item in productToAdd.Photos)
                {
                    photos.Add(new Photos(item));
                }

                product.Photos = photos;
                product.Table = productToAdd.Table;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return Json(new { success = true, responseText = "Your message successfuly sent!" });
            }
            else
            {
                return Json(new { success = false, responseText = "Your message not sent!" });
            }
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var product = _context.Products.Include(c => c.Sizes).Include(k => k.Photos).Single(x => x.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(product);
                }
            }
        }

        [HttpPost]
        public async Task<JsonResult> Edit([FromBody]AddProduct productToAdd)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<Size> prodAtr = new List<Size>();
                    var result = _context.Products.Include(c => c.Photos).Include(c => c.Sizes).
                        FirstOrDefault(x => x.Id == Convert.ToInt32(productToAdd.Id));

                    var quantityWithoutNull = productToAdd.Quantity.Where(x => x != String.Empty).ToArray();
                    for (int i = 0; i < productToAdd.Size.Length; i++)
                    {
                        prodAtr.Add(new Size((SizeOfPruduct)Convert.ToInt32(productToAdd.Size[i]), Convert.ToInt32(quantityWithoutNull[i])));
                    }

                    result.Name = productToAdd.Name;
                    result.Price = Convert.ToDecimal(productToAdd.Price);
                    result.Description = productToAdd.Description;
                    result.Gender = (Gender)Enum.Parse(typeof(Gender), productToAdd.Gender);
                    result.Group = (Group)Convert.ToInt32(productToAdd.Group);
                    result.Sizes = prodAtr;

                    List<Photos> photosList = new List<Photos>();
                    foreach (var photo in productToAdd.Photos)
                    {
                        photosList.Add(new Photos(photo));
                    }
                    result.Photos = photosList;
                    result.Table = productToAdd.Table;
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Json(new { success = false, responseText = "Your message not sent!" });
                }
                return Json(new { success = true, responseText = "Your message successfuly sent!" });
            }
            return Json(new { success = false, responseText = "Your message not sent!" });
        }

        private bool ProductModelExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Products.Include(c => c.Photos).Include(c => c.Sizes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }
            return View(productModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _context.Products.Remove(await _context.Products.Include(c => c.Photos).Include(c => c.Sizes).FirstOrDefaultAsync(i => i.Id == id));
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AllProducts));
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            return View(_userManager.Users.ToList());
        }

        public IActionResult Orders()
        {
            var orders = _context.Orders.Include(c => c.ApplicationUser).Include(c=>c.Products).ThenInclude(d => d.Product).ThenInclude(e => e.Photos).ToList();
            var orderedList = orders.OrderByDescending(x => x.DateOfOrder);
            return View(orderedList);
        }

        public async Task<JsonResult> UpdateDataOrders(string orderId, string orderStatus)
        {
            var Order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId.Trim());
            Order.Status = (Status)Convert.ToInt32(orderStatus);
            _context.Update(Order);
            await _context.SaveChangesAsync();
            return Json(Order);
        }

        public JsonResult GetOrdersInfo()
        {
            var a = _context.Orders.Include(p => p.Products).ToList();
            var result = a.GroupBy(o => o.DateOfOrder).Select(g => new { Date = g.Key, SumMoney = g.Sum(i => Convert.ToDouble(i.Price)) });
            List<OrderChart> orderChart = new List<OrderChart>();
            foreach (var item in result)
            {
                orderChart.Add(new OrderChart(item.Date, item.SumMoney.ToString()));
            }
            orderChart = orderChart.OrderBy(x => x.Date).ToList();
            return Json(orderChart);
        }

        public async Task<JsonResult> AddNewTable()
        {
            var file = Request.Form.Files.First();
            var imagePath = @"\Upload\Tables\";
            var uploadPath = _hostingEnvironmen.WebRootPath + imagePath;

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var uniqueFileName = Guid.NewGuid().ToString();
            var fileName = Path.GetFileName(uniqueFileName + "." + file.FileName.Split(".")[1].ToLower());

            string fullPath = uploadPath + fileName;
            imagePath = imagePath + @"\";

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return Json(new { success = false, responseText = "Table added!" });
        }

        public async Task<JsonResult> AddNewPhotos()
        {
            var file = Request.Form.Files.ToList();

            List<string> listOfPhotos = new List<string>();
            foreach (var item in file)
            {
                var imagePath = @"\Upload\Photos\";
                var uploadPath = _hostingEnvironmen.WebRootPath + imagePath;
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var uniqueFileName = Guid.NewGuid().ToString();
                var fileName = Path.GetFileName(uniqueFileName + "." + item.FileName.Split(".")[1].ToLower());

                string fullPath = uploadPath + fileName;
                var filePath = Path.Combine(imagePath, fileName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await item.CopyToAsync(fileStream);
                }
                listOfPhotos.Add(filePath);
            }
            return Json(listOfPhotos);
        }

        [HttpPost]
        public void RemoveFromDatabase(string path)
        {
            if (!Directory.Exists(path))
            {
                var deletePath = _hostingEnvironmen.WebRootPath + path;
                var fileInfo = new System.IO.FileInfo(deletePath);
                try
                {
                    fileInfo.Delete();
                }
                catch
                {
                    throw new System.InvalidOperationException("Błąd w usuwaniu zdjęcia.");
                }
            }
        }

        public JsonResult GetAllTables()
        {
            var allTables = Directory.GetFiles(_hostingEnvironmen.WebRootPath + @"\Upload\Tables\", "*.*", SearchOption.AllDirectories).ToList();
            foreach (var table in allTables)
            {
                table.Replace(_hostingEnvironmen.WebRootPath, "");
            }
            return Json(allTables);
        }
    }
}
