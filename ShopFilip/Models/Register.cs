using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Models
{
    public class Register
    {
        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [Required, MaxLength(256)]
        public string Surname { get; set; }

        [Required, MaxLength(256)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
