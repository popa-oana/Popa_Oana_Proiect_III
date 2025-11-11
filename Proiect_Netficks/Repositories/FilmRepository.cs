using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_Netficks.Data.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly ApplicationDbContext _context;

        public FilmRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Film>> GetAllFilmsAsync()
        {
            return await _context.Filme
                .Include(f => f.Gen)
                .ToListAsync();
        }

        public async Task<Film?> GetFilmByIdAsync(int id)
        {
            return await _context.Filme
                .Include(f => f.Gen)
                .FirstOrDefaultAsync(f => f.Film_ID == id);
        }

        public async Task<IEnumerable<Film>> SearchFilmsByTitleAsync(string? title)
        {
            if (string.IsNullOrEmpty(title))
                return await GetAllFilmsAsync();

            return await _context.Filme
                .Include(f => f.Gen)
                .Where(f => f.Titlu.Contains(title))
                .ToListAsync();
        }

        public async Task<IEnumerable<Film>> SearchFilmsByGenreAsync(int genreId)
        {
            return await _context.Filme
                .Include(f => f.Gen)
                .Where(f => f.Gen_ID == genreId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Film>> SearchFilmsAsync(string? title, int? year, int? genreId)
        {
            var query = _context.Filme.Include(f => f.Gen).AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(f => f.Titlu.Contains(title));

            if (year.HasValue)
                query = query.Where(f => f.An_Lansare == year.Value);

            if (genreId.HasValue && genreId.Value > 0)
                query = query.Where(f => f.Gen_ID == genreId.Value);

            return await query.ToListAsync();
        }
    }
}
