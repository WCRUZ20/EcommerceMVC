using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Ingresa tu usuario o correo electrónico.")]
    [Display(Name = "Usuario o correo electrónico")]
    [StringLength(256, ErrorMessage = "El usuario o correo electrónico no puede superar {1} caracteres.")]
    public string UserNameOrEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa tu contraseña.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Recordarme")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
