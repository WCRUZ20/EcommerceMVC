using Ecommerce.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models;

public class DiscProduct
{
    [MaxLength(64)]
    public string Sku { get; set; } = string.Empty;

    public string IdUser { get; set; } = string.Empty;

    public decimal RegularPrice { get; set; }

    public decimal DiscountPercent { get; set; }

    public decimal FinalPrice { get; set; }

    public Product? Product { get; set; }

    public ApplicationUser? User { get; set; }
}
