using System.ComponentModel.DataAnnotations;

namespace WebApplicationCore.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember my login on this computer")]
        public bool RememberMe { get; set; }
    }
}
