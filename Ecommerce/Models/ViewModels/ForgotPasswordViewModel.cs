using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.ViewModels;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Ingresa el correo electrónico de tu cuenta.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo electrónico válido.")]
    [Display(Name = "Correo electrónico")]
    [StringLength(256, ErrorMessage = "El correo electrónico no puede superar {1} caracteres.")]
    public string Email { get; set; } = string.Empty;
}
