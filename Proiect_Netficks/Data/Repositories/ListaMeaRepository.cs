using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_Netficks.Data.Repositories
{
    public class ListaMeaRepository : IListaMeaRepository
    {
        private readonly ApplicationDbContext _context;

        public ListaMeaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lista_Mea>> GetUserWatchlistAsync(string userId)
        {
            try
            {
                // Get user's Utilizator record
                var utilizator = await GetUtilizatorByUserIdAsync(userId);
                if (utilizator == null)
                {
                    Console.WriteLine($"No Utilizator found for userId: {userId}");
                    return new List<Lista_Mea>();
                }

                Console.WriteLine($"Found Utilizator with ID: {utilizator.Utilizator_ID} for userId: {userId}");

                // Folosim Include pentru a asigura că toate relațiile sunt încărcate corect
                var result = await _context.ListaMea
                    .Include(lm => lm.Utilizator) // Include utilizatorul
                    .Include(lm => lm.Film)      // Include filmul
                        .ThenInclude(f => f.Gen) // Include genul filmului
                    .Include(lm => lm.Serial)    // Include serialul
                        .ThenInclude(s => s.Gen) // Include genul serialului
                    .Where(lm => lm.Utilizator_ID == utilizator.Utilizator_ID)
                    .ToListAsync();
                    
                // Adăugăm log pentru debugging
                Console.WriteLine($"Found {result.Count} items in watchlist for user {userId}");

                // No longer setting a default status since we now store it in the database
                foreach (var item in result)
                {
                    // Log the item details including Status
                    if (item.Film != null)
                        Console.WriteLine($"Film: {item.Film.Titlu}, Status: {item.Status}");
                    if (item.Serial != null)
                        Console.WriteLine($"Serial: {item.Serial.Titlu}, Status: {item.Status}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving watchlist: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return new List<Lista_Mea>();
            }
        }

        public async Task<Lista_Mea?> GetWatchlistItemAsync(int id)
        {
            return await _context.ListaMea
                .Include(lm => lm.Film)
                .Include(lm => lm.Serial)
                .FirstOrDefaultAsync(lm => lm.Lista_ID == id);
        }
        
        public async Task<Lista_Mea?> GetWatchlistItemByContentAsync(string userId, int? filmId, int? serialId)
        {
            // Get user's Utilizator record
            var utilizator = await GetUtilizatorByUserIdAsync(userId);
            if (utilizator == null)
                return null;
                
            if (filmId.HasValue)
            {
                return await _context.ListaMea
                    .Include(lm => lm.Film)
                    .FirstOrDefaultAsync(lm => lm.Utilizator_ID == utilizator.Utilizator_ID && lm.Film_ID == filmId.Value);
            }
            else if (serialId.HasValue)
            {
                return await _context.ListaMea
                    .Include(lm => lm.Serial)
                    .FirstOrDefaultAsync(lm => lm.Utilizator_ID == utilizator.Utilizator_ID && lm.Serial_ID == serialId.Value);
            }
            
            return null;
        }

        public async Task AddToWatchlistAsync(Lista_Mea item)
        {
            try
            {
                item.Data_Creare = DateTime.Now;
                // Ensure Status is set
                if (item.Status == null)
                {
                    item.Status = VizionareStatus.VreauSaVad;
                }
                
                Console.WriteLine($"Adding to watchlist: Utilizator_ID={item.Utilizator_ID}, Film_ID={item.Film_ID}, Serial_ID={item.Serial_ID}, Status={item.Status}");
                
                await _context.ListaMea.AddAsync(item);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Successfully added item to watchlist with ID: {item.Lista_ID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding to watchlist: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        
        public async Task UpdateWatchlistItemAsync(Lista_Mea item)
        {
            try
            {
                // Ensure Status is set
                if (item.Status == null)
                {
                    item.Status = VizionareStatus.VreauSaVad;
                }
                
                Console.WriteLine($"Updating watchlist item: ID={item.Lista_ID}, Status={item.Status}");
                
                _context.ListaMea.Update(item);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Successfully updated watchlist item with ID: {item.Lista_ID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating watchlist item: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task RemoveFromWatchlistAsync(int id)
        {
            var item = await _context.ListaMea.FindAsync(id);
            if (item != null)
            {
                _context.ListaMea.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsInWatchlistAsync(string userId, int? filmId, int? serialId)
        {
            // Get user's Utilizator record
            var utilizator = await GetUtilizatorByUserIdAsync(userId);
            if (utilizator == null)
                return false;
                
            if (filmId.HasValue)
            {
                return await _context.ListaMea
                    .AnyAsync(lm => lm.Utilizator_ID == utilizator.Utilizator_ID && lm.Film_ID == filmId.Value);
            }
            else if (serialId.HasValue)
            {
                return await _context.ListaMea
                    .AnyAsync(lm => lm.Utilizator_ID == utilizator.Utilizator_ID && lm.Serial_ID == serialId.Value);
            }
            
            return false;
        }
        
        public async Task<Utilizator?> GetUtilizatorByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;
                
            // Get the user from the identity system - direct query without ExecutionStrategy
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return null;
                
            // Find the corresponding Utilizator record by email
            var utilizator = await _context.Set<Utilizator>().FirstOrDefaultAsync(u => u.Email == user.Email);
            
            // If no Utilizator record exists, create one
            if (utilizator == null)
            {
                utilizator = new Utilizator
                {
                    Nume = user.Nume,
                    Email = user.Email ?? string.Empty,
                    Data_Inregistrare = user.Data_Inregistrare,
                    Tip_Abonament = user.Tip_Abonament
                };
                
                _context.Set<Utilizator>().Add(utilizator);
                await _context.SaveChangesAsync();
            }
            
            return utilizator;
        }
    }
}
