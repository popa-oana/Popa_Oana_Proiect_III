using Proiect_Netficks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proiect_Netficks.Data.Repositories
{
    public interface IListaMeaRepository
    {
        Task<IEnumerable<Lista_Mea>> GetUserWatchlistAsync(string userId);
        Task<Lista_Mea?> GetWatchlistItemAsync(int id);
        Task<Lista_Mea?> GetWatchlistItemByContentAsync(string userId, int? filmId, int? serialId);
        Task AddToWatchlistAsync(Lista_Mea item);
        Task UpdateWatchlistItemAsync(Lista_Mea item);
        Task RemoveFromWatchlistAsync(int id);
        Task<bool> IsInWatchlistAsync(string userId, int? filmId, int? serialId);
        Task<Utilizator?> GetUtilizatorByUserIdAsync(string userId);
    }
}
