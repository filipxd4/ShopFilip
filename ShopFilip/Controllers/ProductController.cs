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
                var a = _context.Products.Include(c => c.Sizes).Include(k => k.Photos).Single(x => x.Id == id);
                return View(a);
            }
            return View();
        }

        public async Task<IActionResult> Show(string genre)
        {
            List<Product> ProductsData;
            if (!string.IsNullOrEmpty(genre))
            {
                ProductsData = await _context.Products.Where(m => string.Equals(m.Name, genre, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
            }
            else
            {
                ProductsData = await _context.Products.ToListAsync();
            }
            if (ProductsData == null)
            {
                return NotFound();
            }
            ViewData["Genre"] = genre;
            return View(ProductsData);
        }

        [Route("MenMainPage/{id}")]
        public IActionResult MenMainPage()
        {
            return View();
        }

        [Route("WomanMainPage/{id}")]
        public IActionResult WomanMainPage()
        {
            return View();
        }

        public JsonResult GetQuantityByProductIdAndSize(int id, string size)
        {
            var products = _context.Products.Where(x => x.Id == id).Include(c => c.Sizes).First();
            int quantity = 0;
            foreach (var item in products.Sizes)
            {
                if (item.SizeOfPruduct.ToString() == size)
                {
                    quantity = Convert.ToInt32(item.Quantity);
                }
            }
            return Json(quantity);
        }

        public ActionResult GetPaggedData(string SearchValue, string[] Sizes, string GroupOfProducts, Gender gender, int pageNumber = 1, int pageSize = 2)
        {
            var a = Sizes.ToArray();
            List<Product> tempListOfProducts = new List<Product>();
            List<Product> ProperListOfProducts = new List<Product>();
            bool hasAtribute = false;
            if (SearchValue == null)
            {
                tempListOfProducts.AddRange(from product in _context.Products.Include(photo => photo.Photos).Include(atr => atr.Sizes)
                              where product.Group == (Group)Enum.Parse(typeof(Group), GroupOfProducts) && product.Gender== (Gender)Convert.ToInt32(gender)
                                            select product);
                if (Sizes.Count() != 0)
                {
                    foreach (var item in tempListOfProducts)
                    {
                        foreach (var itemo in item.Sizes)
                        {
                            foreach (var iteam in a)
                            {
                                if ((SizeOfPruduct)Convert.ToInt32(iteam) == itemo.SizeOfPruduct)
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
                tempListOfProducts.AddRange(from product in _context.Products.Include(c => c.Photos).Include(c => c.Sizes)
                              where product.Name.ToLower().Contains(SearchValue.ToLower())&& product.Group == (Group)Enum.Parse(typeof(Group), GroupOfProducts) && product.Gender == gender
                                            select product);
                if (Sizes.Count() != 0)
                {
                    foreach (var item in tempListOfProducts)
                    {
                        foreach (var itemo in item.Sizes)
                        {
                            foreach (var iteam in a)
                            {
                                if ((SizeOfPruduct)Convert.ToInt32(iteam) == itemo.SizeOfPruduct)
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