using System.ComponentModel.DataAnnotations;

namespace Proiect_Netficks.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Numele este obligatoriu")]
        [StringLength(100, ErrorMessage = "Numele nu poate depăși 100 de caractere")]
        [Display(Name = "Nume")]
        public string Nume { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        [EmailAddress(ErrorMessage = "Adresa de email nu este validă")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Numărul de telefon nu este valid")]
        [Display(Name = "Telefon")]
        public string? Telefon { get; set; }
    }
}
