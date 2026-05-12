namespace Ecommerce.Data;

public class AdminUserOptions
{
    public const string SectionName = "AdminUser";

    public string UserName { get; set; } = "admin";

    public string Email { get; set; } = "admin@ecommerce.local";

    public string Password { get; set; } = "Admin123456!";

    public string Role { get; set; } = "Admin";
}
