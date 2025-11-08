using System.ComponentModel.DataAnnotations;

namespace Proiect_Netficks.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Numele este obligatoriu")]
        [StringLength(100, ErrorMessage = "Numele trebuie să aibă între {2} și {1} caractere", MinimumLength = 2)]
        [Display(Name = "Nume")]
        public string Nume { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        [EmailAddress(ErrorMessage = "Adresa de email nu este validă")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parola este obligatorie")]
        [StringLength(100, ErrorMessage = "Parola trebuie să aibă cel puțin {2} caractere", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Parolă")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmă parola")]
        [Compare("Password", ErrorMessage = "Parolele nu coincid")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipul de abonament este obligatoriu")]
        [Display(Name = "Tip Abonament")]
        public string TipAbonament { get; set; } = "Basic";
    }
}
