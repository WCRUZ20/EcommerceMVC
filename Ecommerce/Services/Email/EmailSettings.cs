using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Services.Email;

public class EmailSettings
{
    public const string SectionName = "Email";

    [Required]
    public string Host { get; set; } = string.Empty;

    [Range(1, 65535)]
    public int Port { get; set; } = 587;

    [Required]
    [EmailAddress]
    public string SenderEmail { get; set; } = string.Empty;

    [Required]
    public string SenderName { get; set; } = "Ecommerce";

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool EnableSsl { get; set; } = true;
}
