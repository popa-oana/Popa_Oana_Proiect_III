using Microsoft.AspNetCore.Identity;
using Proiect_Netficks.Models;
using System.Threading.Tasks;
#nullable enable

namespace Proiect_Netficks.Services
{
    public interface IAuthService
    {
        Task<(IdentityResult result, string? userId)> RegisterUserAsync(string email, string password, string nume, string tipAbonament);
        Task<(SignInResult result, string? userId)> LoginUserAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<IdentityResult> AddToRoleAsync(string userId, string role);
        Task<User?> GetUserByIdAsync(string userId);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
