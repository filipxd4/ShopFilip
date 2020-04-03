﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class ProductsId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string IdOfOrder { get; set; }

        public int IdOfProduct { get; set; }

        public string Value { get; set; }

        public int Quantity { get; set; }
    }
}
