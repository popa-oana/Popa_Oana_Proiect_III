using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Data;
using Proiect_Netficks.Models;
using Proiect_Netficks.ViewModels;
using System.Diagnostics;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_Netficks.Controllers
{
    [Authorize]
    public class VizionareController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public VizionareController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Vizionare/Serial/5
        public async Task<IActionResult> Serial(int? id)
        {
            try 
            {
                // Log pentru debug
                Console.WriteLine($"Vizionare serial cu ID: {id}");
                
                if (id == null)
                {
                    return NotFound();
                }

                // Încărcăm datele serialului
                var serial = await _context.Seriale
                    .Include(s => s.Gen)
                    .FirstOrDefaultAsync(s => s.Serial_ID == id);

                if (serial == null)
                {
                    return NotFound();
                }
                
                // Încărcăm toate episoadele pentru acest serial
                var episoade = await _context.Episoade
                    .Where(e => e.Serial_ID == id.Value)
                    .OrderBy(e => e.Numar_Sezon)
                    .ThenBy(e => e.Numar_Episod)
                    .ToListAsync();
                
                Console.WriteLine($"Am găsit {episoade.Count} episoade pentru serialul {serial.Titlu}");
                
                // Dacă nu avem episoade, creăm automat 12 episoade standard cu durata fixă de 30 min
                if (!episoade.Any())
                {
                    Console.WriteLine("Nu există episoade - creăm episoade implicite");
                    
                    for (int i = 1; i <= 12; i++)
                    {
                        var episod = new Episod
                        {
                            Titlu = $"Episodul {i}",
                            Descriere = $"Descriere implicită pentru episodul {i}",
                            Durata = 30, // 30 minute fix pentru toate episoadele
                            Serial_ID = serial.Serial_ID,
                            Numar_Sezon = 1,
                            Numar_Episod = i,
                            VideoUrl = "/assets/videos/placeholder.mp4"
                        };
                        
                        _context.Episoade.Add(episod);
                    }
                    
                    await _context.SaveChangesAsync();
                    
                    // Reîncărcăm episoadele după ce le-am creat
                    episoade = await _context.Episoade
                        .Where(e => e.Serial_ID == id.Value)
                        .OrderBy(e => e.Numar_Sezon)
                        .ThenBy(e => e.Numar_Episod)
                        .ToListAsync();
                        
                    Console.WriteLine($"Am creat {episoade.Count} episoade noi");
                }
                
                // Grupăm episoadele după sezon pentru afișare
                var episoadeGrupate = episoade
                    .GroupBy(e => e.Numar_Sezon)
                    .ToDictionary(g => g.Key, g => g.ToList());
                
                // Creăm modelul pentru view
                var viewModel = new VizionareSerialViewModel
                {
                    Serial = serial,
                    EpisoadeGrupate = episoadeGrupate
                };
                
                // Adaugăm o intrare în istoricul de vizionare (fără a folosi Serial_ID care nu există)
                try
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    if (currentUser != null)
                    {
                        var utilizator = await _context.Set<Utilizator>().FirstOrDefaultAsync(u => u.Email == currentUser.Email);
                        if (utilizator != null)
                        {
                            // Folosim doar primul episod pentru istoric
                            var primulEpisod = episoade.FirstOrDefault();
                            if (primulEpisod != null)
                            {
                                var istoric = new Istoric_Vizionari
                                {
                                    Utilizator_ID = utilizator.Utilizator_ID,
                                    Episod_ID = primulEpisod.Episod_ID,
                                    Film_ID = null,
                                    Timp_Vizionare = 0,
                                    Data_Vizionare = DateTime.Now
                                };
                                
                                _context.IstoricVizionari.Add(istoric);
                                await _context.SaveChangesAsync();
                                Console.WriteLine("Istoric de vizionare creat cu succes");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Doar logăm eroarea, dar continuăm - istoricul nu e critic
                    Console.WriteLine($"Eroare la crearea istoricului: {ex.Message}");
                }
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log eroarea pentru debug
                Console.WriteLine($"Eroare în acțiunea Serial: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        // GET: Vizionare/Film/5
        public async Task<IActionResult> Film(int? id)
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

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            // Adăugăm în istoricul de vizionări
            var utilizator = await _context.Set<Utilizator>().FirstOrDefaultAsync(u => u.Email == currentUser.Email);
            
            if (utilizator == null)
            {
                // Creăm un nou utilizator în tabela Utilizatori legat de User-ul curent
                utilizator = new Utilizator
                {
                    Nume = currentUser.Nume,
                    Email = currentUser.Email ?? string.Empty,
                    Data_Inregistrare = currentUser.Data_Inregistrare,
                    Tip_Abonament = currentUser.Tip_Abonament
                };
                _context.Set<Utilizator>().Add(utilizator);
                await _context.SaveChangesAsync();
            }

            // Verificăm dacă există deja o înregistrare în istoric
            var istoric = await _context.Set<Istoric_Vizionari>()
                .FirstOrDefaultAsync(i => i.Utilizator_ID == utilizator.Utilizator_ID && i.Film_ID == id);

            if (istoric == null)
            {
                // Adăugăm o nouă înregistrare în istoric
                istoric = new Istoric_Vizionari
                {
                    Utilizator_ID = utilizator.Utilizator_ID,
                    Film_ID = film.Film_ID,
                    Timp_Vizionare = 0, // Începem de la 0
                    Data_Vizionare = DateTime.Now
                };
                _context.Set<Istoric_Vizionari>().Add(istoric);
            }
            else
            {
                // Actualizăm data vizionării
                istoric.Data_Vizionare = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            var viewModel = new VizionareFilmViewModel
            {
                Film = film
            };

            return View(viewModel);
        }

        // GET: Vizionare/Episod
        public async Task<IActionResult> Episod(int? serialId, int? episodId)
        {
            if (serialId == null || episodId == null)
            {
                return NotFound();
            }

            var serial = await _context.Seriale
                .Include(s => s.Gen)
                .FirstOrDefaultAsync(m => m.Serial_ID == serialId);

            if (serial == null)
            {
                return NotFound();
            }

            var episod = await _context.Episoade
                .FirstOrDefaultAsync(e => e.Episod_ID == episodId && e.Serial_ID == serialId);

            if (episod == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            // Adăugăm în istoricul de vizionări
            var utilizator = await _context.Set<Utilizator>().FirstOrDefaultAsync(u => u.Email == currentUser.Email);
            
            if (utilizator == null)
            {
                // Creăm un nou utilizator în tabela Utilizatori legat de User-ul curent
                utilizator = new Utilizator
                {
                    Nume = currentUser.Nume,
                    Email = currentUser.Email ?? string.Empty,
                    Data_Inregistrare = currentUser.Data_Inregistrare,
                    Tip_Abonament = currentUser.Tip_Abonament
                };
                _context.Set<Utilizator>().Add(utilizator);
                await _context.SaveChangesAsync();
            }

            // Verificăm dacă există deja o înregistrare în istoric
            var istoric = await _context.Set<Istoric_Vizionari>()
                .FirstOrDefaultAsync(i => i.Utilizator_ID == utilizator.Utilizator_ID && i.Episod_ID == episodId);

            if (istoric == null)
            {
                // Adăugăm o nouă înregistrare în istoric
                istoric = new Istoric_Vizionari
                {
                    Utilizator_ID = utilizator.Utilizator_ID,
                    Episod_ID = episod.Episod_ID,
                    Timp_Vizionare = 0, // Începem de la 0
                    Data_Vizionare = DateTime.Now
                };
                _context.Set<Istoric_Vizionari>().Add(istoric);
            }
            else
            {
                // Actualizăm data vizionării
                istoric.Data_Vizionare = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            var viewModel = new VizionareEpisodViewModel
            {
                Serial = serial,
                Episod = episod
            };

            return View(viewModel);
        }

        // POST: Vizionare/ActualizeazaTimpVizionare
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizeazaTimpVizionare(int? filmId, int? episodId, int timpVizionare)
        {
            if ((filmId == null && episodId == null) || timpVizionare < 0)
            {
                return BadRequest();
            }

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

            Istoric_Vizionari? istoric = null;

            if (filmId.HasValue)
            {
                // Actualizăm istoricul pentru film
                istoric = await _context.Set<Istoric_Vizionari>()
                    .FirstOrDefaultAsync(i => i.Utilizator_ID == utilizator.Utilizator_ID && i.Film_ID == filmId);
            }
            else if (episodId.HasValue)
            {
                // Actualizăm istoricul pentru episod
                istoric = await _context.Set<Istoric_Vizionari>()
                    .FirstOrDefaultAsync(i => i.Utilizator_ID == utilizator.Utilizator_ID && i.Episod_ID == episodId);
            }

            if (istoric != null)
            {
                istoric.Timp_Vizionare = timpVizionare;
                istoric.Data_Vizionare = DateTime.Now;
                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }

            return NotFound();
        }
    }
}
