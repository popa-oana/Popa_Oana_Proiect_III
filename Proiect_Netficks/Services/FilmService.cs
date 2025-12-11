using Proiect_Netficks.Data.Repositories;
using Proiect_Netficks.Models;
using Proiect_Netficks.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proiect_Netficks.Services
{
    public class FilmService : IFilmService
    {
        private readonly IFilmRepository _filmRepository;

        public FilmService(IFilmRepository filmRepository)
        {
            _filmRepository = filmRepository;
        }

        public async Task<IEnumerable<Film>> GetAllFilmsAsync()
        {
            return await _filmRepository.GetAllFilmsAsync();
        }

        public async Task<Film?> GetFilmByIdAsync(int id)
        {
            return await _filmRepository.GetFilmByIdAsync(id);
        }

        public async Task<IEnumerable<Film>> SearchFilmsAsync(string? title, int? year, int? genreId)
        {
            // Handle null parameters to avoid null reference exceptions
            string safeTitle = title ?? string.Empty;
            
            return await _filmRepository.SearchFilmsAsync(safeTitle, year, genreId);
        }
    }
}
