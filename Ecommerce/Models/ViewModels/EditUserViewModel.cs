using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Models.ViewModels;

public class EditUserViewModel
{
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [Display(Name = "Nombre de usuario")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo electrónico válido.")]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El primer nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El primer nombre no puede superar los 100 caracteres.")]
    [Display(Name = "Primer nombre")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "El segundo nombre no puede superar los 100 caracteres.")]
    [Display(Name = "Segundo nombre")]
    public string? MiddleName { get; set; }

    [Required(ErrorMessage = "El primer apellido es obligatorio.")]
    [StringLength(100, ErrorMessage = "El primer apellido no puede superar los 100 caracteres.")]
    [Display(Name = "Primer apellido")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "El segundo apellido no puede superar los 100 caracteres.")]
    [Display(Name = "Segundo apellido")]
    public string? SecondLastName { get; set; }

    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    [Display(Name = "Tipo de documento")]
    public int? TipoDocumentoId { get; set; }

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    [StringLength(30, ErrorMessage = "El número de documento no puede superar los 30 caracteres.")]
    [Display(Name = "Número de documento")]
    public string NumDocumento { get; set; } = string.Empty;

    [Display(Name = "Rol")]
    public string Role { get; set; } = "User";

    public IReadOnlyList<SelectListItem> TipoDocumentoOptions { get; set; } = [];

    public IReadOnlyDictionary<int, int?> CountValidByTipoDocumento { get; set; } = new Dictionary<int, int?>();
}
