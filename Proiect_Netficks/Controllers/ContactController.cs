using Microsoft.AspNetCore.Mvc;
using Proiect_Netficks.ViewModels;
using System.Threading.Tasks;

namespace Proiect_Netficks.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitContact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // In a real application, this would send an email or save to database
            // For now, we'll just show a success message
            TempData["SuccessMessage"] = "Mesajul tău a fost trimis cu succes! Te vom contacta în curând.";
            
            return RedirectToAction(nameof(Index));
        }
    }
}
