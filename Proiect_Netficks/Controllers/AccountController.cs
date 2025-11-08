using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proiect_Netficks.Models;
using Proiect_Netficks.Services;
using Proiect_Netficks.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Proiect_Netficks.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(IAuthService authService, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _authService = authService;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var (result, userId) = await _authService.RegisterUserAsync(
                    model.Email, 
                    model.Password, 
                    model.Nume, 
                    model.TipAbonament);

                if (result.Succeeded)
                {
                    // Assign role based on subscription type
                    string role = model.TipAbonament == "Premium" ? "Premium" : "Basic";
                    await _authService.AddToRoleAsync(userId, role);

                    // Sign in the user
                    var loginResult = await _authService.LoginUserAsync(model.Email, model.Password, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var (result, userId) = await _authService.LoginUserAsync(
                    model.Email, 
                    model.Password, 
                    model.RememberMe);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileViewModel
            {
                Nume = user.Nume,
                Email = user.Email ?? string.Empty,
                TipAbonament = user.Tip_Abonament,
                DataInregistrare = user.Data_Inregistrare.ToString("dd MMMM yyyy"),
                CurrentProfileImagePath = user.ProfileImagePath
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfileImage(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                // Delete old image if exists and not the default
                if (!string.IsNullOrEmpty(user.ProfileImagePath))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, user.ProfileImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save new image
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfileImage.FileName);
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(fileStream);
                }

                // Update user profile image path
                user.ProfileImagePath = "/images/profiles/" + uniqueFileName;
                await _userManager.UpdateAsync(user);

                TempData["SuccessMessage"] = "Imaginea de profil a fost actualizată cu succes!";
            }

            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProfileImage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(user.ProfileImagePath))
            {
                // Delete the image file
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, user.ProfileImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                // Update user record
                user.ProfileImagePath = null;
                await _userManager.UpdateAsync(user);

                TempData["SuccessMessage"] = "Imaginea de profil a fost eliminată!";
            }

            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
