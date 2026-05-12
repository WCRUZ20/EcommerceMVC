using Ecommerce.Data;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Controllers;

[Authorize]
public class ProductsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var products = await context.Products
            .AsNoTracking()
            .GroupJoin(
                context.DiscProducts.AsNoTracking().Where(discount => discount.IdUser == userId),
                product => product.Sku,
                discount => discount.Sku,
                (product, discounts) => new { product, discounts })
            .SelectMany(
                productDiscount => productDiscount.discounts.DefaultIfEmpty(),
                (productDiscount, discount) => new ProductListItemViewModel
                {
                    Sku = productDiscount.product.Sku,
                    LongDescripcion = productDiscount.product.LongDescripcion,
                    ShortDescripcion = productDiscount.product.ShortDescripcion,
                    Stock = productDiscount.product.Stock,
                    RegularPrice = productDiscount.product.RegularPrice,
                    DiscountPercent = discount == null ? null : discount.DiscountPercent,
                    FinalPrice = discount == null ? null : discount.FinalPrice
                })
            .OrderBy(product => product.ShortDescripcion)
            .ThenBy(product => product.Sku)
            .ToListAsync();

        return View(products);
    }
}
