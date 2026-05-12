using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAtUtc { get; set; }
}
