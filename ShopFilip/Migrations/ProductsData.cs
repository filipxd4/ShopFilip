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

            if (context.Products.Count() == 0)
            {
                //First product
                Product product = new Product();
                List<Size> size = new List<Size>();
                size.Add(new Size(SizeOfPruduct.M, 1));
                size.Add(new Size(SizeOfPruduct.L, 1));
                size.Add(new Size(SizeOfPruduct.XL, 1));
                size.Add(new Size(SizeOfPruduct.XXL, 1));

                List<Photos> photosList = new List<Photos>();
                photosList.Add(new Photos(@"\Upload\Photos\2d79fb18-0341-4a14-8da1-f7f3edb90a49.jpg"));
                photosList.Add(new Photos(@"\Upload\Photos\5fb493c8-04b7-4e3d-97d7-bc1cb69dc10a.jpg"));
                product.Photos = photosList;

                product.Name = "Jeansy damskie Rocks";
                product.Price = 299;
                product.Description = "Spodnie damskie Rocks<br />Skład:Bawełna<br /><br />Idealnie dopasowane";
                product.Gender = Gender.Woman;
                product.Group = Group.Spodnie;
                product.Sizes = size;
                product.Table = @"\Upload\Tables\92e81a01-5c46-4687-8a53-5aa182821760.jpg";
                context.Add(product);

                //Second product
                Product product1 = new Product();
                List<Size> size1 = new List<Size>();
                size1.Add(new Size(SizeOfPruduct.M, 1));
                size1.Add(new Size(SizeOfPruduct.L, 1));
                size1.Add(new Size(SizeOfPruduct.XL, 1));
                size1.Add(new Size(SizeOfPruduct.XXL, 1));

                List<Photos> photosList1 = new List<Photos>();
                photosList1.Add(new Photos(@"\Upload\Photos\4c44be83-44bd-4e89-b2e9-5ebb2b41eded.jpg"));
                photosList1.Add(new Photos(@"\Upload\Photos\41efc1cd-a190-4e7b-a7bd-1e51fcd038db.jpg"));
                photosList1.Add(new Photos(@"\Upload\Photos\87c39856-4d41-494f-beda-a84d3db48857.jpg"));
                product1.Photos = photosList1;

                product1.Name = "Marynarka męska";
                product1.Price = 299;
                product1.Description = "Marynarka męska<br />Skład:Bawełna";
                product1.Gender = Gender.Men;
                product1.Group = Group.Kurtki;
                product1.Sizes = size1;
                product1.Table = @"\Upload\Tables\92e81a01-5c46-4687-8a53-5aa182821760.jpg";
                context.Add(product1);
                await context.SaveChangesAsync();
            }
        }
    }
}
