using Microsoft.AspNetCore.Identity;
using ShopFilip.Helpers;
using ShopFilip.IdentityModels;
using ShopFilip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopFilip.Migrations
{
    public class ProductsData
    {
        public static async Task Initialize(EfDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.ProductsData.Count() == 0)
            {
                Product product = new Product();
                List<ProductAtribute> prodAtr = new List<ProductAtribute>();
                prodAtr.Add(new ProductAtribute("prod", "M", 1));
                prodAtr.Add(new ProductAtribute("prod", "L", 1));
                prodAtr.Add(new ProductAtribute("prod", "XL", 1));
                prodAtr.Add(new ProductAtribute("prod", "XXL", 1));

                List<PhotosList> photosList = new List<PhotosList>();
                photosList.Add(new PhotosList("photo", @"\Upload\Photos\2d79fb18-0341-4a14-8da1-f7f3edb90a49.jpg"));
                photosList.Add(new PhotosList("photo", @"\Upload\Photos\5fb493c8-04b7-4e3d-97d7-bc1cb69dc10a.jpg"));
                product.Photos = photosList;

                product.Name = "Jeansy damskie Rocks";
                product.Price = 299;
                product.Description = "Spodnie damskie Rocks<br />Skład:Bawełna<br /><br />Idealnie dopasowane";
                product.Gender = (Gender)0;
                product.Group = "Spodnie";
                product.ProductAtribute = prodAtr;
                product.Table = @"\Upload\Tables\92e81a01-5c46-4687-8a53-5aa182821760.jpg";
                context.Add(product);
                await context.SaveChangesAsync();
            }
        }
    }
}
