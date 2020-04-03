using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class PhotosList
    {
        public int Id { get; set; }

        public string Atribute { get; set; }

        public string Value { get; set; }

        public PhotosList(string atribute, string value)
        {
            this.Atribute = atribute;
            this.Value = value;
        }
    }   
}
