using System.ComponentModel.DataAnnotations;

namespace EcommerceMvc.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un email válido.")]
        [StringLength(256, ErrorMessage = "El email no puede superar 256 caracteres.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}
