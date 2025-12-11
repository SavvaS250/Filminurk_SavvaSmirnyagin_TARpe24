using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.Accounts
{
    public class AddPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Sisesta oma uus parool")]
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
