using ShopFilip.IdentityModels;
using ShopFilip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Helpers
{
    public static class ViewModelFactory
    {
        public static ProductViewModel MapProductToViewModel(Product model)
        {
            ProductViewModel viewModel = new ProductViewModel()
            {
                Name = model.Name,
                Price = model.Price,
                Table= model.Table,
                Gender = model.Gender,
                Description = model.Description,
                Sizes=model.Sizes,
            };
            return viewModel;
        }

        public static ProductViewModel MapProductsToViewModel(IEnumerable<Product> models)
        {
            ProductViewModel viewModel = new ProductViewModel()
            {
                Name = models.First().Name,
                Price = models.First().Price,
                Table= models.First().Table,
                Gender = models.First().Gender,
                Description = models.First().Description,
            };
            return viewModel;
        }

        public static Product MapToModel(ProductViewModel viewModel)
        {
            Product model = new Product()
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                Table = viewModel.Table,
                Gender = viewModel.Gender,
                Description = viewModel.Description,
            };
            return model;
        }
    }
}
