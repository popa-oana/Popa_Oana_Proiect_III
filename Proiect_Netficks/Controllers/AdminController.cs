using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Data;
using Proiect_Netficks.Models;
using System.Threading.Tasks;

namespace Proiect_Netficks.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin
        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // GET: Admin/Statistics
        public async Task<IActionResult> Statistics()
        {
            var viewModel = new AdminStatisticsViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalFilms = await _context.Filme.CountAsync(),
                TotalSeries = await _context.Seriale.CountAsync(),
                TotalReviews = await _context.Recenzii.CountAsync(),
                PremiumUsers = await _userManager.GetUsersInRoleAsync("Premium"),
                BasicUsers = await _userManager.GetUsersInRoleAsync("Basic")
            };

            return View(viewModel);
        }
    }

    public class AdminStatisticsViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalFilms { get; set; }
        public int TotalSeries { get; set; }
        public int TotalReviews { get; set; }
        public System.Collections.Generic.IList<User> PremiumUsers { get; set; } = new System.Collections.Generic.List<User>();
        public System.Collections.Generic.IList<User> BasicUsers { get; set; } = new System.Collections.Generic.List<User>();
    }
}
