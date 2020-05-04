﻿using ShopFilip.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string Group { get; set; }
        public string Table { get; set; }
        public Gender Gender { get; set; }
        public string Description { get; set; }
        public List<ProductAtribute> ProductAtribute { get; set; }
    }
}
