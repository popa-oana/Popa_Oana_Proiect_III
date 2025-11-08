using Microsoft.AspNetCore.Identity;
using Proiect_Netficks.Models;
using System;
using System.Threading.Tasks;
#nullable enable

namespace Proiect_Netficks.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<(IdentityResult result, string? userId)> RegisterUserAsync(string email, string password, string nume, string tipAbonament)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                Nume = nume,
                Tip_Abonament = tipAbonament,
                Data_Inregistrare = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, password);
            return (result, user.Id);
        }

        public async Task<(SignInResult result, string? userId)> LoginUserAsync(string email, string password, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (SignInResult.Failed, string.Empty);
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);
            return (result, user.Id);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<IdentityResult> AddToRoleAsync(string userId, string role)
        {
            // Create role if it doesn't exist
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
