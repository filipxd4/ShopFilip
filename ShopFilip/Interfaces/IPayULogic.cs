using ShopFilip.IdentityModels;
using ShopFilip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Interfaces
{
    public interface IPayULogic
    {
        Task<string> GetStatusOfOrderAsync(string orderId);
        Task<string> GetAccessTokenAsync();
        Task<string> GeneratePayLink(ApplicationUser user, decimal price, List<ShoppingCartItem> itemfromCart, string ip, string accessToken);
    }
}
