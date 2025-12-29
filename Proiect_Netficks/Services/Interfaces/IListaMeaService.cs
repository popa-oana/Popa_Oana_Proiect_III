using Proiect_Netficks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proiect_Netficks.Services.Interfaces
{
    public interface IListaMeaService
    {
        Task<IEnumerable<Lista_Mea>> GetUserWatchlistAsync(string userId);
        Task<Lista_Mea?> GetWatchlistItemAsync(int id);
        Task AddToWatchlistAsync(string userId, int? filmId, int? serialId, VizionareStatus status = VizionareStatus.VreauSaVad);
        Task RemoveFromWatchlistAsync(int id);
        Task<bool> IsInWatchlistAsync(string userId, int? filmId, int? serialId);
        Task UpdateWatchlistStatusAsync(int id, VizionareStatus status);
        Task<Lista_Mea?> GetWatchlistItemByContentAsync(string userId, int? filmId, int? serialId);
    }
}
