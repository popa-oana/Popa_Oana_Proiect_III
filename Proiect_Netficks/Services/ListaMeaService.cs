using Proiect_Netficks.Data.Repositories;
using Proiect_Netficks.Models;
using Proiect_Netficks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proiect_Netficks.Services
{
    public class ListaMeaService : IListaMeaService
    {
        private readonly IListaMeaRepository _listaMeaRepository;

        public ListaMeaService(IListaMeaRepository listaMeaRepository)
        {
            _listaMeaRepository = listaMeaRepository;
        }

        public async Task<IEnumerable<Lista_Mea>> GetUserWatchlistAsync(string userId)
        {
            return await _listaMeaRepository.GetUserWatchlistAsync(userId);
        }

        public async Task<Lista_Mea?> GetWatchlistItemAsync(int id)
        {
            return await _listaMeaRepository.GetWatchlistItemAsync(id);
        }

        public async Task<Lista_Mea?> GetWatchlistItemByContentAsync(string userId, int? filmId, int? serialId)
        {
            // Validate input
            if (string.IsNullOrEmpty(userId) || (!filmId.HasValue && !serialId.HasValue))
                throw new ArgumentException("User ID and either Film ID or Serial ID must be provided.");

            return await _listaMeaRepository.GetWatchlistItemByContentAsync(userId, filmId, serialId);
        }

        public async Task AddToWatchlistAsync(string userId, int? filmId, int? serialId, VizionareStatus status = VizionareStatus.VreauSaVad)
        {
            // Validate input
            if (string.IsNullOrEmpty(userId) || (!filmId.HasValue && !serialId.HasValue))
                throw new ArgumentException("User ID and either Film ID or Serial ID must be provided.");

            try
            {
                Console.WriteLine($"Adding to watchlist: UserId={userId}, FilmId={filmId}, SerialId={serialId}, Status={status}");
                
                // Get the Utilizator record for this user
                var utilizator = await _listaMeaRepository.GetUtilizatorByUserIdAsync(userId);
                if (utilizator == null)
                {
                    throw new InvalidOperationException($"User with ID {userId} not found in the system");
                }

                Console.WriteLine($"Found Utilizator with ID: {utilizator.Utilizator_ID}");

                // Check if already in watchlist
                if (await _listaMeaRepository.IsInWatchlistAsync(userId, filmId, serialId))
                {
                    Console.WriteLine("Item already in watchlist, updating status");
                    
                    // If already in watchlist, update the status
                    var existingItem = await _listaMeaRepository.GetWatchlistItemByContentAsync(userId, filmId, serialId);
                    if (existingItem != null)
                    {
                        existingItem.Status = status;
                        await _listaMeaRepository.UpdateWatchlistItemAsync(existingItem);
                        Console.WriteLine($"Updated watchlist item ID: {existingItem.Lista_ID} with status: {status}");
                    }
                    else
                    {
                        // This shouldn't happen, but log it if it does
                        Console.WriteLine("IsInWatchlist returned true but item not found. This is unexpected.");
                    }
                }
                else
                {
                    Console.WriteLine("Item not in watchlist, adding new entry");
                    
                    // If not in watchlist, add new entry
                    var watchlistItem = new Lista_Mea
                    {
                        Utilizator_ID = utilizator.Utilizator_ID,
                        Film_ID = filmId,
                        Serial_ID = serialId,
                        Data_Creare = DateTime.Now,
                        Status = status
                    };
                    
                    await _listaMeaRepository.AddToWatchlistAsync(watchlistItem);
                    Console.WriteLine($"Added new watchlist item with status: {status}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error adding to watchlist: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw; // Rethrow to let controller handle it
            }
        }

        public async Task RemoveFromWatchlistAsync(int id)
        {
            await _listaMeaRepository.RemoveFromWatchlistAsync(id);
        }

        public async Task<bool> IsInWatchlistAsync(string userId, int? filmId, int? serialId)
        {
            return await _listaMeaRepository.IsInWatchlistAsync(userId, filmId, serialId);
        }

        public async Task UpdateWatchlistStatusAsync(int id, VizionareStatus status)
        {
            var item = await _listaMeaRepository.GetWatchlistItemAsync(id);
            if (item == null)
                throw new ArgumentException("Watchlist item not found.");

            item.Status = status;
            await _listaMeaRepository.UpdateWatchlistItemAsync(item);
        }
    }
}
