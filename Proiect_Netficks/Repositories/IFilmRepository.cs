using Proiect_Netficks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proiect_Netficks.Data.Repositories
{
    public interface IFilmRepository
    {
        Task<IEnumerable<Film>> GetAllFilmsAsync();
        Task<Film?> GetFilmByIdAsync(int id);
        Task<IEnumerable<Film>> SearchFilmsByTitleAsync(string? title);
        Task<IEnumerable<Film>> SearchFilmsByGenreAsync(int genreId);
        Task<IEnumerable<Film>> SearchFilmsAsync(string? title, int? year, int? genreId);
    }
}
