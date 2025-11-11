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
    public class SerialeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SerialeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Seriale
        public async Task<IActionResult> Index()
        {
            var seriale = await _context.Seriale
                .Include(s => s.Gen)
                .ToListAsync();
            return View(seriale);
        }

        // GET: Seriale/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serial = await _context.Seriale
                .Include(s => s.Gen)
                .Include(s => s.Episoade)
                .FirstOrDefaultAsync(m => m.Serial_ID == id);

            if (serial == null)
            {
                return NotFound();
            }

            return View(serial);
        }

        // GET: Seriale/Create
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Create()
        {
            ViewData["Gen_ID"] = new SelectList(_context.Genuri, "Gen_ID", "Nume_Gen");
            return View();
        }

        // POST: Seriale/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([Bind("Serial_ID,Gen_ID,Titlu,An_Aparitie,Numar_Sezoane,Descriere,ImagineUrl,TrailerUrl")] Serial serial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Gen_ID"] = new SelectList(_context.Genuri, "Gen_ID", "Nume_Gen", serial.Gen_ID);
            return View(serial);
        }

        // GET: Seriale/Edit/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Seriale/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(int id, [Bind("Serial_ID,Gen_ID,Titlu,An_Aparitie,Numar_Sezoane,Descriere,ImagineUrl,TrailerUrl")] Serial serial)
        {
            if (id != serial.Serial_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: Seriale/Delete/5
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Seriale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serial = await _context.Seriale.FindAsync(id);
            _context.Seriale.Remove(serial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SerialExists(int id)
        {
            return _context.Seriale.Any(e => e.Serial_ID == id);
        }
    }
}
