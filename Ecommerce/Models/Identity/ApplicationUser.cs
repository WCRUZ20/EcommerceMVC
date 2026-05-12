using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models.Identity;

public class ApplicationUser : IdentityUser
{
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? MiddleName { get; set; }

    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? SecondLastName { get; set; }

    [MaxLength(30)]
    public string TipoDoc { get; set; } = string.Empty;

    [MaxLength(30)]
    public string NumDocumento { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAtUtc { get; set; }
}
