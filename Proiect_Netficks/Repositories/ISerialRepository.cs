using Proiect_Netficks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proiect_Netficks.Data.Repositories
{
    public interface ISerialRepository
    {
        Task<IEnumerable<Serial>> GetAllSerialsAsync();
        Task<Serial?> GetSerialByIdAsync(int id);
        Task<IEnumerable<Serial>> SearchSerialsAsync(string? title, int? year, int? genreId);
    }
}
