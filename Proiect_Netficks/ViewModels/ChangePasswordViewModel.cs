using System.ComponentModel.DataAnnotations;

namespace Proiect_Netficks.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Parola curentă este obligatorie")]
        [DataType(DataType.Password)]
        [Display(Name = "Parola curentă")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parola nouă este obligatorie")]
        [StringLength(100, ErrorMessage = "Parola trebuie să aibă cel puțin {2} caractere.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola nouă")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmă parola nouă")]
        [Compare("NewPassword", ErrorMessage = "Parola nouă și confirmarea acesteia nu se potrivesc.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
