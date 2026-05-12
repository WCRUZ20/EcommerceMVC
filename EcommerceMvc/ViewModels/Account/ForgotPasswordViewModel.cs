using System.ComponentModel.DataAnnotations;

namespace EcommerceMvc.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un email válido.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}
