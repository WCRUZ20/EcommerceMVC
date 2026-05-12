namespace Ecommerce.Models.ViewModels;

public class UserListItemViewModel
{
    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? LastLoginAtUtc { get; set; }

    public IReadOnlyList<string> Roles { get; set; } = [];
}
