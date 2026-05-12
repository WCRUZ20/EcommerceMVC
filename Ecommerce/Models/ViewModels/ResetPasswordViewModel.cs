using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.ViewModels;

public class ResetPasswordViewModel
{
    [Required(ErrorMessage = "Ingresa el correo electrónico de tu cuenta.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo electrónico válido.")]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El token de recuperación es obligatorio.")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa una nueva contraseña.")]
    [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} y máximo {1} caracteres.", MinimumLength = 12)]
    [DataType(DataType.Password)]
    [Display(Name = "Nueva contraseña")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirma la nueva contraseña.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar nueva contraseña")]
    [Compare(nameof(Password), ErrorMessage = "La contraseña y su confirmación no coinciden.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
