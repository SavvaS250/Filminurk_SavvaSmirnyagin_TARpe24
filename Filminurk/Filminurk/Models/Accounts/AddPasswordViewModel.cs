using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.Accounts
{
    public class AddPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Sisesta oma uus parool")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Kirjuta oma uus parool uuseti")]
        [Compare("NewPassword", ErrorMessage = "Paroolid ei kattu, palun proovi uuseti.")]
        public string ConfirmNewPassword { get; set; }
    }
}
