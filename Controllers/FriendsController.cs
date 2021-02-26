using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dejtingsajt.Data;
using dejtingsajt.Models;
using Microsoft.AspNetCore.Identity;

namespace dejtingsajt.Controllers
{
    public class FriendsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FriendsController (ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Friends
        public async Task<IActionResult> AddFriend(String id)
        {
            Friend friend = new Friend
            {
                SenderId = _userManager.GetUserId(User),
                ReceiverId = id,
                isConfirmed = false
            };

            _context.Friends.Add(friend);
            _context.SaveChanges();

            return RedirectToAction("Details", "Users", new { id });
        }



            public async Task<IActionResult> FriendRequest()
            {
           

            var friendRequests = _context.Friends.Where(f => f.ReceiverId.Equals(_userManager.GetUserId(User)) && f.isConfirmed == false ).Select(f => f.SenderId);
            List < ApplicationUser > senders = new List<ApplicationUser>();
           
            foreach (string id in friendRequests)
            {
                var sender = _context.ApplicationUsers.Find(id);
                senders.Add(sender);
            }

            return View(senders);
            }
      
             public async Task<IActionResult> AcceptFriend(String id)
        {
             Friend friend =_context.Friends.Where(f => f.SenderId == id && f.ReceiverId.Equals(_userManager.GetUserId(User))).FirstOrDefault(); 
             friend.isConfirmed = true;
            _context.SaveChanges();
            return RedirectToAction("Details", "Users", new { id });
        }


        public async Task<IActionResult> FriendsList(String id)
        {
            var friendRequests = _context.Friends.Where(f => f.isConfirmed == true && f.ReceiverId ==id || f.isConfirmed == true && f.SenderId==id).ToList();
            List<ApplicationUser> friends = new List<ApplicationUser>();
            foreach (Friend f in friendRequests)
            {

                ApplicationUser user = new ApplicationUser();
                if (f.SenderId == id)
                {
                    user = _context.ApplicationUsers.Find(f.ReceiverId);
                }
                else 
                    {
                    user = _context.ApplicationUsers.Find(f.SenderId);
                }
                friends.Add(user);
            }

            return View(friends);
          
        }    


    }
}
