using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Data;
using Proiect_Netficks.Models;

namespace Proiect_Netficks.Controllers
{
    [Authorize(Policy = "PremiumContent")]
    public class FilmeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FilmeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Filme
        public async Task<IActionResult> Index()
        {
            var filme = await _context.Filme
                .Include(f => f.Gen)
                .ToListAsync();
            return View(filme);
        }

        // GET: Filme/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Filme/Create
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Create()
        {
            ViewData["Gen_ID"] = new SelectList(_context.Genuri, "Gen_ID", "Nume_Gen");
            return View();
        }

        // POST: Filme/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([Bind("Film_ID,Gen_ID,Titlu,An_Lansare,Durata,Descriere")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Gen_ID"] = new SelectList(_context.Genuri, "Gen_ID", "Nume_Gen", film.Gen_ID);
            return View(film);
        }

        // GET: Filme/Edit/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Filme/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int id, [Bind("Film_ID,Gen_ID,Titlu,An_Lansare,Durata,Descriere")] Film film)
        {
            if (id != film.Film_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: Filme/Delete/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Filme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Filme.FindAsync(id);
            _context.Filme.Remove(film);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Filme.Any(e => e.Film_ID == id);
        }
    }
}