using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESourcing.Core.Entities
{
    public class AppUser : IdentityUser
    {

        public AppUser()
        {
            this.IsSeller = false;
            this.IsBuyer = false;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSeller { get; set; }
        public bool IsBuyer { get; set; }

        public bool isAdmin { get; set; }
    }
}
