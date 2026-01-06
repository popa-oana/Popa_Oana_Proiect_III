using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Data;
using Proiect_Netficks.Models;
using Proiect_Netficks.Services;
using Proiect_Netficks.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_Netficks.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;

        public AdminController(ApplicationDbContext context, UserManager<User> userManager, IAuthService authService)
        {
            _context = context;
            _userManager = userManager;
            _authService = authService;
        }

        // GET: Admin
        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var users = await _userManager.Users.ToListAsync();
            
            // Filter out the current admin user from the list
            if (currentUser != null)
            {
                users = users.Where(u => u.Id != currentUser.Id).ToList();
            }
            
            return View(users);
        }
        
        // POST: Admin/UpdateSubscription
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserSubscription(string userId, string subscriptionType)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(subscriptionType))
            {
                return BadRequest("ID utilizator și tipul de abonament sunt obligatorii.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Utilizatorul nu a fost găsit.");
            }

            // Check if the user being modified is an admin
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return BadRequest("Nu puteți modifica abonamentul unui administrator.");
            }

            // Update user's subscription type
            user.Tip_Abonament = subscriptionType;
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
                    Tip = subscriptionType,
                    Data_Start = DateTime.Now,
                    Data_Sfarsit = DateTime.Now.AddMonths(1),
                    Status = "Activ"
                };
                _context.Abonamente.Add(newSubscription);
            }
            else
            {
                // Update existing subscription
                existingSubscription.Tip = subscriptionType;
                existingSubscription.Data_Start = DateTime.Now;
                existingSubscription.Data_Sfarsit = DateTime.Now.AddMonths(1);
                existingSubscription.Status = "Activ";
                _context.Abonamente.Update(existingSubscription);
            }

            await _context.SaveChangesAsync();

            // Update user role based on subscription type
            string newRole = subscriptionType == "Premium" ? "Premium" : subscriptionType == "Standard" ? "Standard" : "Basic";
            
            // Remove existing non-admin roles
            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = userRoles.Where(r => r != "Admin").ToList();
            if (rolesToRemove.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }
            
            // Add new role based on subscription
            await _authService.AddToRoleAsync(user.Id, newRole);

            return RedirectToAction(nameof(Users));
        }
        
        // POST: Admin/DeleteUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("ID utilizator este obligatoriu.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Utilizatorul nu a fost găsit.");
            }

            // Check if the user being deleted is an admin
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return BadRequest("Nu puteți șterge un administrator.");
            }

            // Delete user's related data
            var subscriptions = await _context.Abonamente
                .Where(a => a.UserId == user.Id)
                .ToListAsync();
                
            _context.Abonamente.RemoveRange(subscriptions);
            
            var watchHistory = await _context.IstoricVizionari
                .Where(iv => iv.Utilizator_ID.ToString() == user.Id)
                .ToListAsync();
                
            _context.IstoricVizionari.RemoveRange(watchHistory);
            
            var watchlist = await _context.ListaMea
                .Where(lm => lm.Utilizator_ID.ToString() == user.Id)
                .ToListAsync();
                
            _context.ListaMea.RemoveRange(watchlist);
            
            var reviews = await _context.Recenzii
                .Where(r => r.Utilizator_ID.ToString() == user.Id)
                .ToListAsync();
                
            _context.Recenzii.RemoveRange(reviews);
            
            await _context.SaveChangesAsync();

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Eroare la ștergerea utilizatorului.");
            }

            return RedirectToAction(nameof(Users));
        }

        // GET: Admin/Statistics
        public async Task<IActionResult> Statistics()
        {
            var viewModel = new AdminStatisticsViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalFilms = await _context.Filme.CountAsync(),
                TotalSeries = await _context.Seriale.CountAsync(),
                TotalReviews = await _context.Recenzii.CountAsync(),
                PremiumUsers = await _userManager.GetUsersInRoleAsync("Premium"),
                StandardUsers = await _userManager.GetUsersInRoleAsync("Standard"),
                BasicUsers = await _userManager.GetUsersInRoleAsync("Basic")
            };

            return View(viewModel);
        }
    }

    public class AdminStatisticsViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalFilms { get; set; }
        public int TotalSeries { get; set; }
        public int TotalReviews { get; set; }
        public System.Collections.Generic.IList<User> PremiumUsers { get; set; } = new System.Collections.Generic.List<User>();
        public System.Collections.Generic.IList<User> StandardUsers { get; set; } = new System.Collections.Generic.List<User>();
        public System.Collections.Generic.IList<User> BasicUsers { get; set; } = new System.Collections.Generic.List<User>();
    }
}
