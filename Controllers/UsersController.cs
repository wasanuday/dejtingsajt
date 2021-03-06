﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dejtingsajt.Data;
using dejtingsajt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace dejtingsajt.Controllers
{
 
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.ApplicationUsers.ToListAsync());
        }

        //  // GET: Users/search form
        public async Task<IActionResult> SearchForm()
        {
            return View();
        }

        //POST: Users/ShowSearchResults
        public async Task<IActionResult>ShowSearchResults(String Searchphrase)
        {
            
            return View("Index", await _context.ApplicationUsers.Where(u => u.Name.Contains(Searchphrase)).ToListAsync());
        }

        // GET: Users/Details/5 
        [Authorize]
        public async Task<IActionResult> Details(String id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var msgRequests = (from m in _context.Messages where m.ReceiverId == id select m);
            List<Message> messages = new List<Message>();
            List<ApplicationUser> senders = new List<ApplicationUser>();
            foreach (Message m in msgRequests)
            {
                var sender = _context.ApplicationUsers.Find(m.SenderId);
                senders.Add(sender);
                messages.Add(m);
            }

            Boolean friendRequests = _context.Friends.Where(f => f.SenderId.Equals(_userManager.GetUserId(User)) && f.ReceiverId.Equals(id)).Select(f => f.isConfirmed).FirstOrDefault();

            var ProfileVM = new ProfileViewModel{
                MessagesSender = senders,
                messagesList = messages,
                friend = friendRequests,
                applicationUser =  user
        };
            return View(ProfileVM);
        }



        // GET: Users/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(String id)
        
       {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
          
            var model = new EditProfileViewModel
            {

                Name= user.Name,
                Age = user.Age,
                Gender = user.Gender,
                Photo =user.Photo

            };


            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {

            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    model.Photo = dataStream.ToArray();
                }
            }
            var user = await _userManager.FindByIdAsync(model.Id);
                user.Name = model.Name;
                user.Photo = model.Photo;
                user.Age = model.Age;
                user.Gender = model.Gender;
           
            var resulte = await _userManager.UpdateAsync(user);
            
            if (resulte.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(String id)
        {
            if (id == null)
                        {
                            return NotFound();
                        }

                        var user = await _context.ApplicationUsers
                            .FirstOrDefaultAsync(m => m.Id == id);
                        if (user == null)
                        {
                            return NotFound();
                        }
                          return View(user);
        }

        // POST: Users/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> DeleteConfirmed(String id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            _context.ApplicationUsers.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(String id)
        {
            return _context.ApplicationUsers.Any(e => e.Id == id);
        }
    }
}
