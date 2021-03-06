﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dejtingsajt.Models
{
    public class ProfileViewModel
    {
        public ApplicationUser applicationUser { get; set; }
        public Boolean friend { get; set; }
        public List<ApplicationUser> MessagesSender { get; set; }
        public List<Message> messagesList { get; set; }
    }
}
