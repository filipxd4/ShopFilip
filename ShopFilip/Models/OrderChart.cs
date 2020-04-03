using ShopFilip.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class OrderChart
    {
        public DateTime Date { get; set; }

        public string SumOfMoney { get; set; }

        public OrderChart(DateTime date, string money)
        {
            this.Date = date;
            this.SumOfMoney= money;
        }
    }
}
