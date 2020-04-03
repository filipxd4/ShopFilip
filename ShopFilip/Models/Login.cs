using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Należy wpisać email!"), MaxLength(256)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Należy wpisać hasło!"), DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
