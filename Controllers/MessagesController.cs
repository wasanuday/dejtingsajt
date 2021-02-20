using dejtingsajt.Data;
using dejtingsajt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;


namespace dejtingsajt.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Route("sendMessage")]
        [HttpPost]
        public void PostMessage(Message msg)
        {
            msg.SenderId = _userManager.GetUserId(User);
            msg.Time = DateTime.Now;
            _context.Add(msg);
            _context.SaveChanges();  
        }
       
    }

}
