using System.ComponentModel.DataAnnotations;

namespace EcommerceMvc.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario o email es obligatorio.")]
        [StringLength(256, ErrorMessage = "El usuario o email no puede superar 256 caracteres.")]
        [Display(Name = "Usuario o email")]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(128, MinimumLength = 12, ErrorMessage = "La contraseña debe tener entre 12 y 128 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }
}
