using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Data;
using Proiect_Netficks.Models;
using Proiect_Netficks.ViewModels;

namespace Proiect_Netficks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var homeViewModel = new HomeViewModel
            {
                // Get the most popular films based on ratings
                PopularFilms = await _context.Filme
                    .Include(f => f.Recenzii)
                    .Include(f => f.Gen)
                    .OrderByDescending(f => f.Recenzii.Count > 0 ? f.Recenzii.Average(r => r.Nota) : 0)
                    .Take(6)
                    .ToListAsync(),
                
                // Get the newest films
                NewestFilms = await _context.Filme
                    .Include(f => f.Gen)
                    .OrderByDescending(f => f.An_Lansare)
                    .Take(6)
                    .ToListAsync(),
                
                // Get the most popular serials
                PopularSerials = await _context.Seriale
                    .Include(s => s.Recenzii)
                    .Include(s => s.Gen)
                    .OrderByDescending(s => s.Recenzii.Count > 0 ? s.Recenzii.Average(r => r.Nota) : 0)
                    .Take(6)
                    .ToListAsync(),
                
                // Get the newest serials
                NewestSerials = await _context.Seriale
                    .Include(s => s.Gen)
                    .OrderByDescending(s => s.An_Aparitie)
                    .Take(6)
                    .ToListAsync()
            };
            
            // If user is logged in, get their viewing history
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var utilizator = await _context.Set<Utilizator>()
                        .FirstOrDefaultAsync(u => u.Email == user.Email);
                    
                    if (utilizator != null)
                    {
                        // Get user's watchlist items with status
                        homeViewModel.Watchlist = await _context.ListaMea
                            .Include(l => l.Film)
                            .Include(l => l.Serial)
                            .Where(l => l.Utilizator_ID == utilizator.Utilizator_ID)
                            .OrderByDescending(l => l.Data_Creare)
                            .Take(6)
                            .ToListAsync();
                    }
                }
            }
            
            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult RemoveFromHistory(int historyId)
        {
            // In a real implementation, this would connect to a service to remove the item
            // For now, we'll just redirect back to the home page with a success message
            TempData["SuccessMessage"] = "Element eliminat cu succes din istoricul tău!";
            
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
