using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models;

public class Product
{
    public int Id { get; set; }

    [MaxLength(64)]
    public string Sku { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string LongDescripcion { get; set; } = string.Empty;

    [MaxLength(255)]
    public string ShortDescripcion { get; set; } = string.Empty;

    public int Stock { get; set; }

    public decimal RegularPrice { get; set; }
}
