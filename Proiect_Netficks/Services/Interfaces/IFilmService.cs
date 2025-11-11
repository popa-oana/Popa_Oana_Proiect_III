using Proiect_Netficks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proiect_Netficks.Services.Interfaces
{
    public interface IFilmService
    {
        Task<IEnumerable<Film>> GetAllFilmsAsync();
        Task<Film?> GetFilmByIdAsync(int id);
        Task<IEnumerable<Film>> SearchFilmsAsync(string? title, int? year, int? genreId);
    }
}
