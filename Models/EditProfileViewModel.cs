using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dejtingsajt.Models
{
    public class EditProfileViewModel
    {
        public ApplicationUser CurrentUser { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public byte[] Photo { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
    }
}
