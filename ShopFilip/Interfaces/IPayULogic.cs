using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Interfaces
{
    public interface IPayULogic
    {
        Task<string> GetStatusOfOrderAsync(string orderId);
    }
}
