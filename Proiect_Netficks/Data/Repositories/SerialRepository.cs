using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_Netficks.Data.Repositories
{
    public class SerialRepository : ISerialRepository
    {
        private readonly ApplicationDbContext _context;

        public SerialRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Serial>> GetAllSerialsAsync()
        {
            try
            {
                return await _context.Seriale
                    .Include(s => s.Gen)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving serials: {ex.Message}");
                return new List<Serial>();
            }
        }

        public async Task<Serial?> GetSerialByIdAsync(int id)
        {
            try
            {
                return await _context.Seriale
                    .Include(s => s.Gen)
                    .FirstOrDefaultAsync(s => s.Serial_ID == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving serial with ID {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<Serial>> SearchSerialsAsync(string? title, int? year, int? genreId)
        {
            try
            {
                var query = _context.Seriale.Include(s => s.Gen).AsQueryable();

                // Filter by title if provided
                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(s => s.Titlu.Contains(title));
                }

                // Filter by year if provided (pentru seriale folosim An_Aparitie)
                if (year.HasValue)
                {
                    query = query.Where(s => s.An_Aparitie == year.Value);
                }

                // Filter by genre if provided
                if (genreId.HasValue)
                {
                    query = query.Where(s => s.Gen_ID == genreId.Value);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching serials: {ex.Message}");
                return new List<Serial>();
            }
        }
    }
}
