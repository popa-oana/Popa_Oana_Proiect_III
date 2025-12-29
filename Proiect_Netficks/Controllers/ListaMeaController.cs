using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proiect_Netficks.Models;
using Proiect_Netficks.Services.Interfaces;
using Proiect_Netficks.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_Netficks.Controllers
{
    [Authorize]
    public class ListaMeaController : Controller
    {
        private readonly IListaMeaService _listaMeaService;

        public ListaMeaController(IListaMeaService listaMeaService)
        {
            _listaMeaService = listaMeaService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var watchlistItems = await _listaMeaService.GetUserWatchlistAsync(userId);
            
            var viewModel = new ListaMeaViewModel
            {
                WatchlistItems = watchlistItems.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromWatchlist(int id)
        {
            await _listaMeaService.RemoveFromWatchlistAsync(id);
            
            // Adaugă un mesaj de succes
            TempData["SuccessMessage"] = "Element eliminat cu succes din lista ta!";
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddToWatchlist(int? filmId, int? serialId, int status = 1)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            VizionareStatus vizStatus = (VizionareStatus)status;
            await _listaMeaService.AddToWatchlistAsync(userId, filmId, serialId, vizStatus);
            
            // Add success message
            TempData["SuccessMessage"] = "Element adăugat cu succes în lista ta!";
            
            // Return to the previous page if available, otherwise go to watchlist
            if (Request.Headers["Referer"].ToString() != null && !Request.Headers["Referer"].ToString().Contains("ListaMea"))
                return Redirect(Request.Headers["Referer"].ToString());
            else
                return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, int status)
        {
            VizionareStatus vizStatus = (VizionareStatus)status;
            await _listaMeaService.UpdateWatchlistStatusAsync(id, vizStatus);
            
            // Add success message
            TempData["SuccessMessage"] = "Status actualizat cu succes!";
            
            // Return to the previous page if available, otherwise go to watchlist
            if (Request.Headers["Referer"].ToString() != null)
                return Redirect(Request.Headers["Referer"].ToString());
            else
                return RedirectToAction(nameof(Index));
        }
    }
}
