using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Proiect_Netficks.ViewModels
{
    public class ProfileViewModel
    {
        [Display(Name = "Imagine de profil")]
        public IFormFile? ProfileImage { get; set; }
        
        public string? CurrentProfileImagePath { get; set; }
        
        [Display(Name = "Nume")]
        public string Nume { get; set; } = string.Empty;
        
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        
        [Display(Name = "Tip abonament")]
        public string TipAbonament { get; set; } = string.Empty;
        
        [Display(Name = "Data înregistrării")]
        public string DataInregistrare { get; set; } = string.Empty;
    }
}
