using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.Accounts
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Kirjuta oma uus parool uuseti")]
        [Compare("NewPassword", ErrorMessage = "Paroolid ei kattu, palun proovi uuseti.")]
        public string ConfirmNewPassword { get; set; }
        public string Token { get; set; }
    }
}
