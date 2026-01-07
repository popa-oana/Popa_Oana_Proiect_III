using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Data;
using Proiect_Netficks.Models;
using System;
using System.Threading.Tasks;

namespace Proiect_Netficks.Controllers
{
    [Authorize]
    public class RecenziiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public RecenziiController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: Recenzii/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int? filmId, int? serialId, int? episodId)
        {
            var recenzie = await _context.Recenzii.FindAsync(id);
            if (recenzie == null)
            {
                return NotFound();
            }
            
            // Verify the user is authorized to delete this review
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            
            // Find the Utilizator ID for this user
            var utilizator = await _context.Set<Utilizator>().FirstOrDefaultAsync(u => u.Email == currentUser.Email);
            if (utilizator == null || utilizator.Utilizator_ID != recenzie.Utilizator_ID)
            {
                // Only allow users to delete their own reviews (or admins)
                if (!User.IsInRole("Admin"))
                {
                    TempData["ErrorMessage"] = "Nu ai permisiunea să ștergi această recenzie.";
                    
                    if (filmId.HasValue)
                    {
                        return RedirectToAction("FilmDetails", "Titluri", new { id = filmId.Value });
                    }
                    else if (serialId.HasValue)
                    {
                        return RedirectToAction("SerialDetails", "Titluri", new { id = serialId.Value });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Titluri");
                    }
                }
            }
            
            // Delete the review
            _context.Recenzii.Remove(recenzie);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Recenzia a fost ștearsă cu succes!";
            
            // Redirect to appropriate page
            if (filmId.HasValue)
            {
                return RedirectToAction("FilmDetails", "Titluri", new { id = filmId.Value });
            }
            else if (serialId.HasValue)
            {
                return RedirectToAction("SerialDetails", "Titluri", new { id = serialId.Value });
            }
            else
            {
                return RedirectToAction("Index", "Titluri");
            }
        }
        
        // POST: Recenzii/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int nota, string comentariu, int? filmId, int? serialId, int? episodId)
        {
            var recenzie = await _context.Recenzii.FindAsync(id);
            if (recenzie == null)
            {
                return NotFound();
            }
            
            // Verify the user is authorized to edit this review
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            
            // Find the Utilizator ID for this user
            var utilizator = await _context.Set<Utilizator>().FirstOrDefaultAsync(u => u.Email == currentUser.Email);
            if (utilizator == null || utilizator.Utilizator_ID != recenzie.Utilizator_ID)
            {
                // Only allow users to edit their own reviews (or admins)
                if (!User.IsInRole("Admin"))
                {
                    TempData["ErrorMessage"] = "Nu ai permisiunea să editezi această recenzie.";
                    
                    if (filmId.HasValue)
                    {
                        return RedirectToAction("FilmDetails", "Titluri", new { id = filmId.Value });
                    }
                    else if (serialId.HasValue)
                    {
                        return RedirectToAction("SerialDetails", "Titluri", new { id = serialId.Value });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Titluri");
                    }
                }
            }
            
            // Update the review
            recenzie.Nota = nota;
            recenzie.Comentariu = comentariu;
            recenzie.Data_Postarii = DateTime.Now;
            
            _context.Update(recenzie);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Recenzia a fost actualizată cu succes!";
            
            // Redirect to appropriate page
            if (filmId.HasValue)
            {
                return RedirectToAction("FilmDetails", "Titluri", new { id = filmId.Value });
            }
            else if (serialId.HasValue)
            {
                return RedirectToAction("SerialDetails", "Titluri", new { id = serialId.Value });
            }
            else
            {
                return RedirectToAction("Index", "Titluri");
            }
        }
    }
}
