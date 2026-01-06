using System;
using System.ComponentModel.DataAnnotations;

namespace Proiect_Netficks.ViewModels
{
    public class SetariViewModel
    {
        public DetaliiPersonaleViewModel DetaliiPersonale { get; set; } = new DetaliiPersonaleViewModel();
        public SchimbaParolaViewModel SchimbaParola { get; set; } = new SchimbaParolaViewModel();
    }

    public class DetaliiPersonaleViewModel
    {
        [Required(ErrorMessage = "Numele este obligatoriu")]
        [Display(Name = "Nume complet")]
        public string Nume { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username-ul este obligatoriu")]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        [EmailAddress(ErrorMessage = "Adresa de email nu este validă")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numărul de telefon este obligatoriu")]
        [Phone(ErrorMessage = "Numărul de telefon nu este valid")]
        [Display(Name = "Telefon")]
        public string Telefon { get; set; } = string.Empty;
    }

    public class SchimbaParolaViewModel
    {
        [Required(ErrorMessage = "Parola veche este obligatorie")]
        [DataType(DataType.Password)]
        [Display(Name = "Parola veche")]
        public string ParolaVeche { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parola nouă este obligatorie")]
        [StringLength(100, ErrorMessage = "Parola trebuie să aibă cel puțin {2} caractere", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola nouă")]
        public string ParolaNoua { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmă parola nouă")]
        [Compare("ParolaNoua", ErrorMessage = "Parolele nu coincid")]
        public string ConfirmaParolaNoua { get; set; } = string.Empty;
    }
}
