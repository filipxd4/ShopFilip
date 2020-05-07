using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class Photos
    {
        public int Id { get; set; }

        public string PhotoPath { get; set; }

        public Photos(string photoPath)
        {
            this.PhotoPath = photoPath;
        }
    }   
}
