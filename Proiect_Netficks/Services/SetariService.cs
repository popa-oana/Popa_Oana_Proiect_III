using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Data;
using Proiect_Netficks.Models;
using Proiect_Netficks.Services.Interfaces;
using Proiect_Netficks.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Proiect_Netficks.Services
{
    public class SetariService : ISetariService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public SetariService(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<SetariViewModel> GetSetariViewModel(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("Utilizatorul nu a fost găsit", nameof(userId));
            }

            return new SetariViewModel
            {
                DetaliiPersonale = new DetaliiPersonaleViewModel
                {
                    Nume = user.Nume,
                    Username = user.UserName,
                    Email = user.Email,
                    Telefon = user.PhoneNumber
                }
            };
        }

        public async Task<bool> UpdateDetaliiPersonale(string userId, DetaliiPersonaleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("Utilizatorul nu a fost găsit", nameof(userId));
            }

            user.Nume = model.Nume;
            user.Email = model.Email;
            user.UserName = model.Username;
            user.PhoneNumber = model.Telefon;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
} 