using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Data;
using Proiect_Netficks.Models;
using Proiect_Netficks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_Netficks.Controllers
{
    [Authorize]
    public class IstoricController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public IstoricController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Istoric
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            // Obținem utilizatorul din tabela Utilizator
            var utilizator = await _context.Set<Utilizator>().FirstOrDefaultAsync(u => u.Email == currentUser.Email);
            
            if (utilizator == null)
            {
                // Dacă nu există, îl creăm
                utilizator = new Utilizator
                {
                    Nume = currentUser.Nume ?? currentUser.UserName ?? "Utilizator",
                    Email = currentUser.Email ?? string.Empty,
                    Data_Inregistrare = currentUser.Data_Inregistrare,
                    Tip_Abonament = currentUser.Tip_Abonament
                };
                _context.Set<Utilizator>().Add(utilizator);
                await _context.SaveChangesAsync();
            }

            // Obținem istoricul de vizionări pentru filme
            var istoricFilme = await _context.Set<Istoric_Vizionari>()
                .Where(i => i.Utilizator_ID == utilizator.Utilizator_ID && i.Film_ID != null)
                .Include(i => i.Film)
                .OrderByDescending(i => i.Data_Vizionare)
                .Take(20)
                .ToListAsync();

            // Obținem istoricul de vizionări pentru episoade
            var istoricEpisoade = await _context.Set<Istoric_Vizionari>()
                .Where(i => i.Utilizator_ID == utilizator.Utilizator_ID && i.Episod_ID != null)
                .Include(i => i.Episod)
                    .ThenInclude(e => e != null ? e.Serial : null)
                .OrderByDescending(i => i.Data_Vizionare)
                .Take(20)
                .ToListAsync();

            var viewModel = new IstoricViewModel
            {
                IstoricFilme = istoricFilme,
                IstoricEpisoade = istoricEpisoade
            };

            return View(viewModel);
        }

        // POST: Istoric/Sterge/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sterge(int id)
        {
            var istoric = await _context.Set<Istoric_Vizionari>().FindAsync(id);
            if (istoric == null)
            {
                return NotFound();
            }

            // Verificăm dacă utilizatorul curent este proprietarul înregistrării
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var utilizator = await _context.Set<Utilizator>().FirstOrDefaultAsync(u => u.Email == currentUser.Email);
            if (utilizator == null || istoric.Utilizator_ID != utilizator.Utilizator_ID)
            {
                return Unauthorized();
            }

            _context.Set<Istoric_Vizionari>().Remove(istoric);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Înregistrarea a fost ștearsă cu succes din istoric.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Istoric/StergeTot
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StergeTot()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var utilizator = await _context.Set<Utilizator>().FirstOrDefaultAsync(u => u.Email == currentUser.Email);
            if (utilizator == null)
            {
                return NotFound();
            }

            var istoricUtilizator = await _context.Set<Istoric_Vizionari>()
                .Where(i => i.Utilizator_ID == utilizator.Utilizator_ID)
                .ToListAsync();

            _context.Set<Istoric_Vizionari>().RemoveRange(istoricUtilizator);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Istoricul de vizionări a fost șters complet.";
            return RedirectToAction(nameof(Index));
        }
    }
}
