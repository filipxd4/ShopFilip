using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopFilip;
using ShopFilip.Helpers;
using ShopFilip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly EfDbContext _context;

        public ProductController(EfDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ProductInfo(int id)
        {
            if (id != 0)
            {
                var a = _context.ProductsData.Include(c => c.ProductAtribute).Include(k => k.Photos).Single(x => x.Id == id);
                return View(a);
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(ProductViewModel viewModel)
        {
            return RedirectToAction("Buy", "ShoppingCart", new { id = viewModel.Id });
        }

        public async Task<IActionResult> Show(string genre)
        {
            List<Product> ProductsData;
            if (!string.IsNullOrEmpty(genre))
            {
                ProductsData = await _context.ProductsData.Where(m => string.Equals(m.Name, genre, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
            }
            else
            {
                ProductsData = await _context.ProductsData.ToListAsync();
            }
            if (ProductsData == null)
            {
                return NotFound();
            }
            ViewData["Genre"] = genre;
            return View(ProductsData);
        }

        [Route("MenMainPage/{id}")]
        public IActionResult MenMainPage(int? pageNumber, string id)
        {
            return View();
        }

        [Route("WomanMainPage/{id}")]
        public IActionResult WomanMainPage(int? pageNumber, string id)
        {
            return View();
        }

        public JsonResult GetQuantityByIdAndAtr(int id, string atr)
        {
            var a = _context.ProductsData.Where(x => x.Id == id).Include(c => c.ProductAtribute).First();
            int quantity = 0;
            foreach (var item in a.ProductAtribute)
            {
                if (item.Value == atr)
                {
                    quantity = Convert.ToInt32(item.Quantity);
                }
            }
            return Json(quantity);
        }

        public ActionResult GetPaggedData(string SearchValue, string[] Sizes, string GroupOfProducts, string gender, int pageNumber = 1, int pageSize = 2)
        {
            var a = Sizes.ToArray();
            List<Product> tempListOfProducts = new List<Product>();
            List<Product> ProperListOfProducts = new List<Product>();
            bool hasAtribute = false;
            if (SearchValue == null)
            {
                tempListOfProducts.AddRange(from product in _context.ProductsData.Include(photo => photo.Photos).Include(atr => atr.ProductAtribute)
                              where product.Group == GroupOfProducts && product.Gender==gender
                                            select product);
                if (Sizes.Count() != 0)
                {
                    foreach (var item in tempListOfProducts)
                    {
                        foreach (var itemo in item.ProductAtribute)
                        {
                            foreach (var iteam in a)
                            {
                                if (iteam == itemo.Value)
                                {
                                    ProperListOfProducts.Add(item);
                                    hasAtribute = true;
                                    break;
                                }
                            }
                            if (hasAtribute)
                            {
                                hasAtribute = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    ProperListOfProducts.AddRange(tempListOfProducts);
                }

                var pagedData = Pagination.PagedResult(ProperListOfProducts, pageNumber, pageSize);
                return Json(pagedData);
            }
            else
            {
                tempListOfProducts.AddRange(from product in _context.ProductsData.Include(c => c.Photos).Include(c => c.ProductAtribute)
                              where product.Name.ToLower().Contains(SearchValue.ToLower())&& product.Group == GroupOfProducts && product.Gender == gender
                                            select product);
                if (Sizes.Count() != 0)
                {
                    foreach (var item in tempListOfProducts)
                    {
                        foreach (var itemo in item.ProductAtribute)
                        {
                            foreach (var iteam in a)
                            {
                                if (iteam == itemo.Value)
                                {
                                    ProperListOfProducts.Add(item);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ProperListOfProducts.AddRange(tempListOfProducts);
                }
                var pagedData = Pagination.PagedResult(ProperListOfProducts, pageNumber, pageSize);
                return Json(pagedData);
            }
        }
    }
}