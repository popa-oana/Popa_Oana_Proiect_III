using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Data;
using Proiect_Netficks.Models;
using Proiect_Netficks.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Proiect_Netficks.Controllers
{
    [Authorize]
    public class TitluriController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;

        public TitluriController(
            ApplicationDbContext context, 
            IWebHostEnvironment webHostEnvironment,
            UserManager<User> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // GET: Titluri
        public async Task<IActionResult> Index()
        {
            var filme = await _context.Filme
                .Include(f => f.Gen)
                .ToListAsync();

            var seriale = await _context.Seriale
                .Include(s => s.Gen)
                .ToListAsync();

            var viewModel = new TitluriViewModel
            {
                Filme = filme,
                Seriale = seriale
            };

            return View(viewModel);
        }

        // GET: Titluri/FilmDetails/5
        public async Task<IActionResult> FilmDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Load film with reviews
                var film = await _context.Filme
                    .Include(f => f.Gen)
                    .Include(f => f.Recenzii)
                        .ThenInclude(r => r.Utilizator)
                    .AsNoTracking() // Prevent tracking issues
                    .FirstOrDefaultAsync(m => m.Film_ID == id);

                if (film == null)
                {
                    return NotFound();
                }
                
                // Debug logs
                Console.WriteLine($"Film găsit: {film.Titlu} (ID: {film.Film_ID})");
                
                // If Recenzii is null, load them separately
                if (film.Recenzii == null || !film.Recenzii.Any())
                {
                    var recenzii = await _context.Recenzii
                        .Include(r => r.Utilizator)
                        .Where(r => r.Film_ID == id)
                        .AsNoTracking()
                        .ToListAsync();
                        
                    film.Recenzii = recenzii;
                    Console.WriteLine($"Loaded {recenzii.Count} reviews separately for film ID: {id}");
                }
                else
                {
                    Console.WriteLine($"Film has {film.Recenzii.Count} reviews already loaded");
                }
                
                foreach (var recenzie in film.Recenzii)
                {
                    Console.WriteLine($"Review ID: {recenzie.Recenzie_ID}, Rating: {recenzie.Nota}, User: {recenzie.Utilizator?.Nume}");
                }

                var viewModel = new FilmDetailsViewModel
                {
                    Film = film,
                    RecenzieNoua = new RecenzieViewModel
                    {
                        Film_ID = film.Film_ID
                    }
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FilmDetails: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Titluri/SerialDetails/5
        public async Task<IActionResult> SerialDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Folosim o abordare mai detaliată cu tracking explicit
                var serial = await _context.Seriale
                    .Include(s => s.Gen)
                    .AsNoTracking() // Prevent tracking issues
                    .FirstOrDefaultAsync(m => m.Serial_ID == id);
                    
                if (serial == null)
                {
                    return NotFound();
                }
                
                // Log pentru debug
                Console.WriteLine($"Serial găsit: {serial.Titlu} (ID: {serial.Serial_ID})");
                
                // Încărcăm episoadele
                var episoade = await _context.Episoade
                    .Where(e => e.Serial_ID == id)
                    .AsNoTracking()
                    .ToListAsync();
                    
                Console.WriteLine($"Found {episoade.Count} episodes for serial ID: {id}");
                
                // Încărcăm recenziile specifice pentru acest serial
                var recenzii = await _context.Recenzii
                    .Include(r => r.Utilizator)
                    .Where(r => r.Serial_ID == id)
                    .AsNoTracking()
                    .ToListAsync();
                    
                Console.WriteLine($"Found {recenzii.Count} reviews for serial ID: {id}");
                
                foreach (var recenzie in recenzii)
                {
                    Console.WriteLine($"Review ID: {recenzie.Recenzie_ID}, Rating: {recenzie.Nota}, User: {recenzie.Utilizator?.Nume}");
                }
                
                // Asociem manual datele încărcate
                serial.Episoade = episoade;
                serial.Recenzii = recenzii;
                
                var viewModel = new SerialDetailsViewModel
                {
                    Serial = serial,
                    RecenzieNoua = new RecenzieViewModel
                    {
                        Serial_ID = serial.Serial_ID
                    }
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SerialDetails: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Titluri/CreateFilm
        [Authorize(Policy = "AdminOnly")]
        public IActionResult CreateFilm()
        {
            ViewData["Gen_ID"] = new SelectList(_context.Genuri, "Gen_ID", "Nume_Gen");
            return View();
        }

        // POST: Titluri/CreateFilm
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateFilm([Bind("Film_ID,Gen_ID,Titlu,An_Lansare,Durata,Descriere,TrailerUrl")] Film film, IFormFile Imagine)
        {
            if (ModelState.IsValid)
            {
                // Process image if uploaded
                if (Imagine != null && Imagine.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Imagine.FileName);
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "filme");
                    
                    // Create directory if it doesn't exist
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Imagine.CopyToAsync(fileStream);
                    }

                    film.ImagineUrl = "/images/filme/" + uniqueFileName;
                }

                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Gen_ID"] = new SelectList(_context.Genuri, "Gen_ID", "Nume_Gen", film.Gen_ID);
            return View(film);
        }

        // GET: Titluri/CreateSerial
        [Authorize(Policy = "AdminOnly")]
        public IActionResult CreateSerial()
        {
            ViewData["Gen_ID"] = new SelectList(_context.Genuri, "Gen_ID", "Nume_Gen");
            return View();
        }

        // POST: Titluri/CreateSerial
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateSerial([Bind("Serial_ID,Gen_ID,Titlu,An_Aparitie,Numar_Sezoane,Descriere,TrailerUrl,ImagineUrl")] Serial serial, IFormFile Imagine)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Process image if uploaded
                    if (Imagine != null && Imagine.Length > 0)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Imagine.FileName);
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "seriale");
                        
                        // Create directory if it doesn't exist
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await Imagine.CopyToAsync(fileStream);
                        }

                        serial.ImagineUrl = "/images/seriale/" + uniqueFileName;
                    }
                    else
                    {
                        // Set default image if no image was uploaded
                        serial.ImagineUrl = "/images/placeholder-serial.jpg";
                    }

                    _context.Add(serial);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "A apărut o eroare la salvarea serialului. Vă rugăm să încercați din nou.");
                    // Log the error
                    Console.WriteLine($"Error creating serial: {ex.Message}");
                }
            }

            // If we got this far, something failed
            ViewData["Gen_ID"] = new SelectList(_context.Genuri, "Gen_ID", "Nume_Gen", serial.Gen_ID);
            return View(serial);
        }

        // GET: Titluri/EditFilm/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> EditFilm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Filme.FindAsync(id);

            if (film == null)
            {
                return NotFound();
            }

            ViewData["Gen_ID"] = new SelectList(_context.Genuri, "Gen_ID", "Nume_Gen", film.Gen_ID);
            return View(film);
        }

        // POST: Titluri/EditFilm/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> EditFilm(int id, [Bind("Film_ID,Gen_ID,Titlu,An_Lansare,Durata,Descriere,ImagineUrl,TrailerUrl")] Film film, IFormFile Imagine)
        {
            if (id != film.Film_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Process image if uploaded
                    if (Imagine != null && Imagine.Length > 0)
                    {
                        // Delete old image if exists and not the default
                        if (!string.IsNullOrEmpty(film.ImagineUrl) && !film.ImagineUrl.Contains("placeholder"))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, film.ImagineUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Imagine.FileName);
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "filme");
                        
                        // Create directory if it doesn't exist
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await Imagine.CopyToAsync(fileStream);
                        }

                        film.ImagineUrl = "/images/filme/" + uniqueFileName;
                    }

                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Film_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Gen_ID"] = new SelectList(_context.Genuri, "Gen_ID", "Nume_Gen", film.Gen_ID);
            return View(film);
        }

        // GET: Titluri/EditSerial/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> EditSerial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serial = await _context.Seriale.FindAsync(id);

            if (serial == null)
            {
                return NotFound();
            }

            ViewData["Gen_ID"] = new SelectList(_context.Genuri, "Gen_ID", "Nume_Gen", serial.Gen_ID);
            return View(serial);
        }

        // POST: Titluri/EditSerial/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> EditSerial(int id, [Bind("Serial_ID,Gen_ID,Titlu,An_Aparitie,Numar_Sezoane,Descriere,ImagineUrl,TrailerUrl")] Serial serial, IFormFile Imagine)
        {
            if (id != serial.Serial_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Process image if uploaded
                    if (Imagine != null && Imagine.Length > 0)
                    {
                        // Delete old image if exists and not the default
                        if (!string.IsNullOrEmpty(serial.ImagineUrl) && !serial.ImagineUrl.Contains("placeholder"))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, serial.ImagineUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Imagine.FileName);
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "seriale");
                        
                        // Create directory if it doesn't exist
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await Imagine.CopyToAsync(fileStream);
                        }

                        serial.ImagineUrl = "/images/seriale/" + uniqueFileName;
                    }

                    _context.Update(serial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SerialExists(serial.Serial_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Gen_ID"] = new SelectList(_context.Genuri, "Gen_ID", "Nume_Gen", serial.Gen_ID);
            return View(serial);
        }

        // GET: Titluri/DeleteFilm/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteFilm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Filme
                .Include(f => f.Gen)
                .FirstOrDefaultAsync(m => m.Film_ID == id);

            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Titluri/DeleteFilm/5
        [HttpPost, ActionName("DeleteFilm")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteFilmConfirmed(int id)
        {
            var film = await _context.Filme.FindAsync(id);
            
            // Delete image if exists and not the default
            if (film != null && !string.IsNullOrEmpty(film.ImagineUrl) && !film.ImagineUrl.Contains("placeholder"))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, film.ImagineUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            
            if (film != null)
            {
                _context.Filme.Remove(film);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Titluri/DeleteSerial/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteSerial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serial = await _context.Seriale
                .Include(s => s.Gen)
                .FirstOrDefaultAsync(m => m.Serial_ID == id);

            if (serial == null)
            {
                return NotFound();
            }

            return View(serial);
        }

        // POST: Titluri/DeleteSerial/5
        [HttpPost, ActionName("DeleteSerial")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteSerialConfirmed(int id)
        {
            var serial = await _context.Seriale.FindAsync(id);
            
            // Delete image if exists and not the default
            if (serial != null && !string.IsNullOrEmpty(serial.ImagineUrl) && !serial.ImagineUrl.Contains("placeholder"))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, serial.ImagineUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            
            if (serial != null)
            {
                _context.Seriale.Remove(serial);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Titluri/AdaugaRecenzie
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AdaugaRecenzie(RecenzieViewModel model)
        {
            Console.WriteLine($"=== REVIEW SUBMISSION STARTED ====");
            Console.WriteLine($"Review data: Film_ID={model.Film_ID}, Serial_ID={model.Serial_ID}, Episod_ID={model.Episod_ID}, Nota={model.Nota}");
            Console.WriteLine($"Comment: {model.Comentariu?.Substring(0, Math.Min(model.Comentariu?.Length ?? 0, 50))}{(model.Comentariu?.Length > 50 ? "..." : "")}");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    Console.WriteLine($"ModelState error for {state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }
            
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                
                if (currentUser == null)
                {
                    Console.WriteLine("Error: Current user is null");
                    return Unauthorized();
                }

                Console.WriteLine($"Current user: ID={currentUser.Id}, Email={currentUser.Email}, Name={currentUser.Nume}");

                // Verificăm dacă există un utilizator în tabela Utilizatori cu același email
                var utilizator = await _context.Set<Utilizator>().FirstOrDefaultAsync(u => u.Email == currentUser.Email);
                
                if (utilizator == null)
                {
                    Console.WriteLine("Creating new Utilizator record for user...");
                    // Creăm un nou utilizator în tabela Utilizatori legat de User-ul curent
                    utilizator = new Utilizator
                    {
                        Nume = currentUser.Nume ?? currentUser.UserName ?? "Utilizator",
                        Email = currentUser.Email ?? string.Empty,
                        Parola = "IdentityManaged", // Necesar pentru constrângerea Required
                        Data_Inregistrare = DateTime.Now,
                        Tip_Abonament = currentUser.Tip_Abonament ?? "Basic"
                    };
                    
                    _context.Set<Utilizator>().Add(utilizator);
                    await _context.SaveChangesAsync();
                    
                    Console.WriteLine($"Created new Utilizator with ID: {utilizator.Utilizator_ID}");
                }
                else
                {
                    Console.WriteLine($"Found existing Utilizator with ID: {utilizator.Utilizator_ID}");
                }
                
                // Validate that we have at least one content ID
                if (!model.Film_ID.HasValue && !model.Serial_ID.HasValue && !model.Episod_ID.HasValue)
                {
                    Console.WriteLine("Error: No content ID provided (Film_ID, Serial_ID, or Episod_ID)");
                    ModelState.AddModelError(string.Empty, "Trebuie să specifici un film, serial sau episod pentru recenzie");
                    TempData["ErrorMessage"] = "Recenzia nu a putut fi adăugată: lipsesc date necesare."; 
                    return RedirectToAction(nameof(Index));
                }
                
                // Check if user already has a review for this content
                Recenzii? existingReview = null;
                
                if (model.Film_ID.HasValue)
                {
                    existingReview = await _context.Recenzii
                        .FirstOrDefaultAsync(r => r.Utilizator_ID == utilizator.Utilizator_ID && r.Film_ID == model.Film_ID);
                        
                    Console.WriteLine($"Searching for film review: Utilizator_ID={utilizator.Utilizator_ID}, Film_ID={model.Film_ID}");
                }
                else if (model.Serial_ID.HasValue)
                {
                    Console.WriteLine($"Searching for serial review: Utilizator_ID={utilizator.Utilizator_ID}, Serial_ID={model.Serial_ID}");
                    existingReview = await _context.Recenzii
                        .FirstOrDefaultAsync(r => r.Utilizator_ID == utilizator.Utilizator_ID && 
                                                 r.Serial_ID == model.Serial_ID);
                        
                    if (existingReview != null)
                        Console.WriteLine($"Found existing review: ID={existingReview.Recenzie_ID}, Nota={existingReview.Nota}");
                    else
                        Console.WriteLine("No existing review found for this serial");
                }
                else if (model.Episod_ID.HasValue)
                {
                    existingReview = await _context.Recenzii
                        .FirstOrDefaultAsync(r => r.Utilizator_ID == utilizator.Utilizator_ID && r.Episod_ID == model.Episod_ID);
                        
                    Console.WriteLine($"Searching for episode review: Utilizator_ID={utilizator.Utilizator_ID}, Episod_ID={model.Episod_ID}");
                }
                
                if (existingReview != null)
                {
                    // Update existing review
                    Console.WriteLine($"Updating existing review ID: {existingReview.Recenzie_ID}");
                    Console.WriteLine($"Old values: Nota={existingReview.Nota}, Comment={existingReview.Comentariu?.Substring(0, Math.Min(existingReview.Comentariu?.Length ?? 0, 20))}");
                    Console.WriteLine($"New values: Nota={model.Nota}, Comment={model.Comentariu?.Substring(0, Math.Min(model.Comentariu?.Length ?? 0, 20))}");
                    
                    existingReview.Nota = model.Nota;
                    existingReview.Comentariu = model.Comentariu;
                    existingReview.Data_Postarii = DateTime.Now;
                    
                    _context.Recenzii.Update(existingReview);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Recenzia ta a fost actualizată cu succes!";
                    Console.WriteLine($"Successfully updated review ID: {existingReview.Recenzie_ID}");
                }
                else
                {
                    // Creăm o nouă recenzie
                    Console.WriteLine("Creating new review...");
                    var recenzie = new Recenzii
                    {
                        Utilizator_ID = utilizator.Utilizator_ID,
                        Film_ID = model.Film_ID,
                        Serial_ID = model.Serial_ID,
                        Episod_ID = model.Episod_ID,
                        Nota = model.Nota,
                        Comentariu = model.Comentariu,
                        Data_Postarii = DateTime.Now
                    };
                    
                    Console.WriteLine($"New review values: Utilizator_ID={utilizator.Utilizator_ID}, Film_ID={model.Film_ID}, Serial_ID={model.Serial_ID}, Episod_ID={model.Episod_ID}, Nota={model.Nota}");

                    _context.Recenzii.Add(recenzie);
                    var saveResult = await _context.SaveChangesAsync();
                    Console.WriteLine($"Database SaveChanges result: {saveResult} records affected");
                    
                    if (recenzie.Recenzie_ID > 0)
                    {
                        TempData["SuccessMessage"] = "Recenzia ta a fost adăugată cu succes!";
                        Console.WriteLine($"Successfully created new review with ID: {recenzie.Recenzie_ID}");
                    }
                    else
                    {
                        Console.WriteLine("Warning: Review was saved but no ID was generated");
                    }
                }

                Console.WriteLine("=== REVIEW SUBMISSION COMPLETED SUCCESSFULLY ===");
                
                // Return to the appropriate page
                if (model.Film_ID.HasValue)
                {
                    Console.WriteLine($"Redirecting to FilmDetails with ID: {model.Film_ID}");
                    return RedirectToAction(nameof(FilmDetails), new { id = model.Film_ID });
                }
                else if (model.Serial_ID.HasValue)
                {
                    Console.WriteLine($"Redirecting to SerialDetails with ID: {model.Serial_ID}");
                    return RedirectToAction(nameof(SerialDetails), new { id = model.Serial_ID });
                }
                else if (model.Episod_ID.HasValue)
                {
                    // For episodes, we need to find the serial it belongs to
                    var episod = await _context.Episoade.FindAsync(model.Episod_ID);
                    if (episod?.Serial_ID != null)
                    {
                        Console.WriteLine($"Redirecting to SerialDetails with ID: {episod.Serial_ID} (from Episod_ID: {model.Episod_ID})");
                        return RedirectToAction(nameof(SerialDetails), new { id = episod.Serial_ID });
                    }
                }
                
                Console.WriteLine("No specific content ID to redirect to, going to Index");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in AdaugaRecenzie: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError(string.Empty, $"A apărut o eroare: {ex.Message}");
                TempData["ErrorMessage"] = $"A apărut o eroare: {ex.Message}";
                
                // Try to redirect back to the page they were on
                if (model.Film_ID.HasValue)
                    return RedirectToAction(nameof(FilmDetails), new { id = model.Film_ID });
                else if (model.Serial_ID.HasValue)
                    return RedirectToAction(nameof(SerialDetails), new { id = model.Serial_ID });
                else
                    return RedirectToAction(nameof(Index));
            }
            
            // If we got here, something failed in ModelState validation
            Console.WriteLine("ModelState validation failed, attempting to redisplay form");

            // If we got this far, something failed, redisplay form
            if (model.Film_ID.HasValue)
            {
                var film = await _context.Filme
                    .Include(f => f.Gen)
                    .Include(f => f.Recenzii)
                        .ThenInclude(r => r.Utilizator)
                    .FirstOrDefaultAsync(m => m.Film_ID == model.Film_ID);

                var viewModel = new FilmDetailsViewModel
                {
                    Film = film,
                    RecenzieNoua = model
                };

                return View("FilmDetails", viewModel);
            }
            else if (model.Serial_ID.HasValue)
            {
                var serial = await _context.Seriale
                    .Include(s => s.Gen)
                    .Include(s => s.Episoade)
                    .Include(s => s.Recenzii)
                        .ThenInclude(r => r.Utilizator)
                    .FirstOrDefaultAsync(m => m.Serial_ID == model.Serial_ID);

                var viewModel = new SerialDetailsViewModel
                {
                    Serial = serial,
                    RecenzieNoua = model
                };

                return View("SerialDetails", viewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Filme.Any(e => e.Film_ID == id);
        }

        private bool SerialExists(int id)
        {
            return _context.Seriale.Any(e => e.Serial_ID == id);
        }
    }
}
