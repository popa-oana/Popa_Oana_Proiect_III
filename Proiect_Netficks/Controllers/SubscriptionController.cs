using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Models;
using Proiect_Netficks.Services;
using Proiect_Netficks.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Proiect_Netficks.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;
        private readonly Data.ApplicationDbContext _context;

        public SubscriptionController(
            UserManager<User> userManager,
            IAuthService authService,
            Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _authService = authService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Check if user is admin
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            ViewBag.IsAdmin = isAdmin;
            ViewBag.CurrentPlan = user.Tip_Abonament;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSubscription(string planType)
        {
            if (string.IsNullOrEmpty(planType))
            {
                return BadRequest("Tipul de abonament nu poate fi gol.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Check if user is admin - admins cannot change their subscription type
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return Json(new { success = false, message = "Administratorii nu își pot schimba tipul de abonament." });
            }

            // Update user's subscription type
            user.Tip_Abonament = planType;
            await _userManager.UpdateAsync(user);

            // Create or update Abonament record
            var existingSubscription = await _context.Abonamente
                .FirstOrDefaultAsync(a => a.UserId == user.Id);

            if (existingSubscription == null)
            {
                // Create new subscription
                var newSubscription = new Abonament
                {
                    UserId = user.Id,
                    Tip = planType,
                    Data_Start = DateTime.Now,
                    Data_Sfarsit = DateTime.Now.AddMonths(1),
                    Status = "Activ"
                };
                _context.Abonamente.Add(newSubscription);
            }
            else
            {
                // Update existing subscription
                existingSubscription.Tip = planType;
                existingSubscription.Data_Start = DateTime.Now;
                existingSubscription.Data_Sfarsit = DateTime.Now.AddMonths(1);
                existingSubscription.Status = "Activ";
                _context.Abonamente.Update(existingSubscription);
            }

            await _context.SaveChangesAsync();

            // Preserve admin role if user is an admin
            var userRoles = await _userManager.GetRolesAsync(user);
            bool isAdmin = userRoles.Contains("Admin");

            // Determine new role based on subscription type
            string newRole = planType == "Premium" ? "Premium" : "Basic";
            
            // Remove existing non-admin roles
            var rolesToRemove = userRoles.Where(r => r != "Admin").ToList();
            if (rolesToRemove.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }
            
            // Add new role based on subscription
            if (!isAdmin) // Don't add subscription role if user is admin
            {
                await _authService.AddToRoleAsync(user.Id, newRole);
            }

            // Return JSON response for the AJAX call
            return Json(new { success = true, plan = planType });
        }
    }
}
