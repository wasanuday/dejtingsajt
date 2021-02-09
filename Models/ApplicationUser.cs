using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dejtingsajt.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }

        public ApplicationUser() { }
  }
}
