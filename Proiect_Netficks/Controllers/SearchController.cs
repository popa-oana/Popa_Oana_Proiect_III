using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proiect_Netficks.Data.Repositories;
using Proiect_Netficks.Models;
using Proiect_Netficks.Services.Interfaces;
using Proiect_Netficks.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_Netficks.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly IFilmService _filmService;
        private readonly ISerialService _serialService;
        private readonly IListaMeaService _listaMeaService;
        private readonly IFilmRepository _filmRepository;
        private readonly ISerialRepository _serialRepository;

        public SearchController(
            IFilmService filmService, 
            ISerialService serialService,
            IListaMeaService listaMeaService,
            IFilmRepository filmRepository,
            ISerialRepository serialRepository)
        {
            _filmService = filmService;
            _serialService = serialService;
            _listaMeaService = listaMeaService;
            _filmRepository = filmRepository;
            _serialRepository = serialRepository;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new SearchViewModel
            {
                Genres = await GetGenresSelectList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchViewModel model)
        {
            // Get user ID
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            // Adăugăm log pentru debugging
            Console.WriteLine($"Searching for title: {model.Title}, year: {model.Year}, genreId: {model.GenreId}");
            
            // 1. Căutare filme
            var films = await _filmService.SearchFilmsAsync(
                model.Title ?? string.Empty, 
                model.Year, 
                model.GenreId);
                
            Console.WriteLine($"Found {films.Count()} films");
            
            // Verificăm care filme sunt în watchlist
            var searchResults = new List<SearchResultViewModel>();
            foreach (var film in films)
            {
                var isInWatchlist = await _listaMeaService.IsInWatchlistAsync(userId, film.Film_ID, null);
                searchResults.Add(new SearchResultViewModel
                {
                    Film = film,
                    IsInWatchlist = isInWatchlist
                });
            }
            
            // 2. Căutare seriale
            var serials = await _serialService.SearchSerialsAsync(
                model.Title ?? string.Empty,
                model.Year,
                model.GenreId);
                
            Console.WriteLine($"Found {serials.Count()} serials");
            
            // Verificăm care seriale sunt în watchlist
            var serialResults = new List<SerialSearchResultViewModel>();
            foreach (var serial in serials)
            {
                var isInWatchlist = await _listaMeaService.IsInWatchlistAsync(userId, null, serial.Serial_ID);
                serialResults.Add(new SerialSearchResultViewModel
                {
                    Serial = serial,
                    IsInWatchlist = isInWatchlist
                });
            }

            // Actualizăm modelul cu rezultatele
            model.SearchResults = searchResults;
            model.SerialResults = serialResults;
            model.Genres = await GetGenresSelectList();

            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWatchlist(int filmId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            await _listaMeaService.AddToWatchlistAsync(userId, filmId, null);
            
            // Adaugă un mesaj de succes
            TempData["SuccessMessage"] = "Film adăugat cu succes în lista ta!";
            
            // Redirecționează înapoi la pagina de căutare
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> AddSerialToWatchlist(int serialId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            await _listaMeaService.AddToWatchlistAsync(userId, null, serialId);
            
            // Adaugă un mesaj de succes
            TempData["SuccessMessage"] = "Serial adăugat cu succes în lista ta!";
            
            // Redirecționează înapoi la pagina de căutare
            return RedirectToAction("Index");
        }

        private async Task<SelectList> GetGenresSelectList()
        {
            try
            {
                // Get all genres from the database
                var films = await _filmRepository.GetAllFilmsAsync();
                
                // Filter out null genres and get distinct genres
                var genresList = films.Where(f => f.Gen != null)
                                      .Select(f => f.Gen)
                                      .Distinct()
                                      .ToList();
                
                return new SelectList(genresList, "Gen_ID", "Nume_Gen");
            }
            catch (System.Exception ex)
            {
                // Log the exception
                System.Console.WriteLine($"Error in GetGenresSelectList: {ex.Message}");
                
                // Return an empty SelectList to avoid crashing
                return new SelectList(System.Linq.Enumerable.Empty<SelectListItem>());
            }
        }
    }
}
