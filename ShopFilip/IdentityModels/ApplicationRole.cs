using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.IdentityModels
{
    public class ApplicationRole: IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName, DateTime creationDate) : base(roleName)
        {
            this.CreationDate = creationDate;
        }
        public DateTime CreationDate { get; set; }
    }
}
