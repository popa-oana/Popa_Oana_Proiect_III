using System.ComponentModel.DataAnnotations;

namespace Proiect_Netficks.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Numele este obligatoriu")]
        [Display(Name = "Nume complet")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Adresa de email este obligatorie")]
        [EmailAddress(ErrorMessage = "Adresa de email nu este validă")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Subiectul este obligatoriu")]
        [Display(Name = "Subiect")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Mesajul este obligatoriu")]
        [Display(Name = "Mesaj")]
        [MinLength(10, ErrorMessage = "Mesajul trebuie să conțină cel puțin 10 caractere")]
        [MaxLength(1000, ErrorMessage = "Mesajul nu poate depăși 1000 de caractere")]
        public string? Message { get; set; }
    }
}
