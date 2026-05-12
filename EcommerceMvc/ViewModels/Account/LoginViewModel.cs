using System.ComponentModel.DataAnnotations;

namespace EcommerceMvc.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario o email es obligatorio.")]
        [Display(Name = "Usuario o email")]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }
}
