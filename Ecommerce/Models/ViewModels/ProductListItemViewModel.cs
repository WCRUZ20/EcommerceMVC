namespace Ecommerce.Models.ViewModels;

public class ProductListItemViewModel
{
    public string Sku { get; set; } = string.Empty;

    public string LongDescripcion { get; set; } = string.Empty;

    public string ShortDescripcion { get; set; } = string.Empty;

    public int Stock { get; set; }

    public decimal RegularPrice { get; set; }

    public decimal? DiscountPercent { get; set; }

    public decimal? FinalPrice { get; set; }
}
