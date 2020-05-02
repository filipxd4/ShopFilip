using Microsoft.AspNetCore.Identity;
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

            //if (await roleManager.FindByNameAsync(role1) == null)
            //{
            //    await roleManager.CreateAsync(new ApplicationRole(role1, desc1, DateTime.Now));
            //}
            //if (await roleManager.FindByNameAsync(role2) == null)
            //{
            //    await roleManager.CreateAsync(new ApplicationRole(role2, desc2, DateTime.Now));
            //}

            //if (await userManager.FindByNameAsync("aa@aa.aa") == null)
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "aa@aa.aa",
            //        Email = "aa@aa.aa",
            //    };

            //    var result = await userManager.CreateAsync(user);
            //    if (result.Succeeded)
            //    {
            //        await userManager.AddPasswordAsync(user, password);
            //        //await userManager.AddToRoleAsync(user, role1);
            //        await userManager.AddClaimAsync(user, claim: new Claim(ClaimTypes.Role.ToString(), "Admin"));
            //    }
            //    adminId1 = user.Id;
            //}
        }
    }
}
