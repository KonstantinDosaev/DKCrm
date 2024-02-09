using System.ComponentModel.DataAnnotations;

namespace DKCrm.Shared.Models.UserAuth
{
    public class RegisterRequest
    {
        [Required] public string UserName { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Пароль не совпадает!")]
        public string PasswordConfirm { get; set; } = null!;
    }
}
