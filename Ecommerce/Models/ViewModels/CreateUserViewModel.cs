using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.ViewModels;

public class CreateUserViewModel
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [Display(Name = "Nombre de usuario")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo electrónico válido.")]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña temporal")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Rol")]
    public string Role { get; set; } = "User";
}
